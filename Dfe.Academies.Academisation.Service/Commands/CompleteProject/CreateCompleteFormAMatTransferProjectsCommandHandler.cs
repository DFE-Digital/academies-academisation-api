using System.Net.Http.Json;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Commands.CompleteProject.Helpers;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;
using Dfe.Complete.Client.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateCompleteFormAMatTransferProjectsCommandHandler(
		ITransferProjectRepository transferProjectRepository,
		IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
		IAcademiesQueryService academiesQueryService,
		ICompleteTransmissionLogRepository completeTransmissionLogRepository,
		IDateTimeProvider dateTimeProvider,
		IPollyPolicyFactory pollyPolicyFactory,
		ILogger<CreateCompleteFormAMatTransferProjectsCommandHandler> logger,
		IProjectsClient projectsClient) : IRequestHandler<CreateCompleteFormAMatTransferProjectsCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(CreateCompleteFormAMatTransferProjectsCommand request,
			CancellationToken cancellationToken)
		{
			var retryPolicy = pollyPolicyFactory.GetCompleteHttpClientRetryPolicy(logger);

			var transferProjects = await transferProjectRepository.GetFormAMatProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if (transferProjects == null || !transferProjects.Any())
			{
				logger.LogInformation($"No transfer projects found.");
				return new NotFoundCommandResult();
			}

			var transferingAcademies = transferProjects.SelectMany(x => x.TransferringAcademies,
				(transferProject, academy) => new
				{
					academy.Id,
					TransferProjectId = transferProject.Id,
					Ukprn = academy.OutgoingAcademyUkprn,
					Include = !academy.ProjectSentToComplete
				}).Where(x => x.Include);

			var establishments = await academiesQueryService.GetBulkEstablishmentsByUkprn(transferingAcademies.Select(x => x.Ukprn));

			foreach (var transferringAcademy in transferingAcademies)
			{
				var transferProject = transferProjects.Single(x => x.Id == transferringAcademy.TransferProjectId);
				var decision = await advisoryBoardDecisionRepository.GetTransferProjectDecsion(transferringAcademy.TransferProjectId);
				var establishment = establishments.Single(x => x.Ukprn == transferringAcademy.Ukprn);

				var transferObject = CompleteTransferProjectServiceModelMapper.FormAMatFromDomain(transferProject,
					decision?.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails!, establishment.Urn);
				 
				var response = await CompleteApiHelper.ExecuteCompleteClientCallAsync(
					transferObject,
					async (req, ct) => await projectsClient.CreateTransferMatProjectAsync(req, ct).ConfigureAwait(false),
					id => new CreateCompleteConversionProjectSuccessResponse(id!.Value.GetValueOrDefault()),
					retryPolicy,
					cancellationToken);

				Guid? completeProjectId = null;
				string responseMessage = string.Empty;

				if (response.IsSuccessStatusCode)
				{
					var successResponse =
						await response.Content.ReadFromJsonAsync<CreateCompleteTransferProjectSuccessResponse>(cancellationToken: cancellationToken);

					completeProjectId = successResponse?.transfer_project_id;

					logger.LogInformation(
						"Success sending transfer to complete with project urn: {Urn} with Status code 201 ",
						transferProject.Urn);
				}
				else
				{
					var errorResponse =
						await response.Content.ReadFromJsonAsync<CreateCompleteProjectErrorResponse>(cancellationToken);
					responseMessage = errorResponse?.Response ?? "No error message returned";
					logger.LogError("Error sending transfer project to complete with project urn: {Project} for transfering academy: {Urn} due to Status code {Code} and Complete Validation Errors: {Response}", transferProject.Urn, establishment.Urn, response.StatusCode, responseMessage);
				}

				transferProject.SetProjectSentToComplete(transferringAcademy.Ukprn);
				 
				if (transferProject is Domain.TransferProjectAggregate.TransferProject domainProject)
				{
					transferProjectRepository.Update(domainProject);
				}
				await transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				completeTransmissionLogRepository.Insert(CompleteTransmissionLog.CreateTransferProjectLog(transferProject.Id, transferringAcademy.Id, completeProjectId, response.IsSuccessStatusCode, responseMessage, dateTimeProvider.Now));
				await completeTransmissionLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			}

			return new CommandSuccessResult();
		}

	}
}
