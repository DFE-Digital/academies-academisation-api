using System.Net.Http.Json;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;
using Dfe.Complete.Client.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateCompleteTransferProjectsCommandHandler(
		ITransferProjectRepository transferProjectRepository,
		IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
		IAcademiesQueryService academiesQueryService, 
		ICompleteTransmissionLogRepository completeTransmissionLogRepository, 
		IDateTimeProvider dateTimeProvider,
		ICompleteApiClientRetryFactory completeApiClientRetryFactory,
		ILogger<CreateCompleteTransferProjectsCommandHandler> logger) : IRequestHandler<CreateCompleteTransferProjectsCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(CreateCompleteTransferProjectsCommand request,
			CancellationToken cancellationToken)
		{ 
			var retryPolicy = completeApiClientRetryFactory.GetCompleteHttpClientRetryPolicy(logger);

			var transferProjects = await transferProjectRepository.GetProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

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

				var transferObject = CompleteTransferProjectServiceModelMapper.FromDomain(transferProject,
					decision?.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails!, establishment.Urn);

				var response = await completeApiClientRetryFactory.CreateTransferProjectAsync(
					transferObject, 
					retryPolicy,
					cancellationToken);

				Guid? completeProjectId = null;
				string responseMessage = string.Empty;

				if (response.IsSuccessStatusCode)
				{
					var successResponse =
						await response.Content.ReadFromJsonAsync<CreateCompleteTransferProjectSuccessResponse>(cancellationToken);
					completeProjectId = successResponse?.transfer_project_id;

					logger.LogInformation(
						"Success sending transfer project to complete with project urn: {Urn} with Status code 201 ",
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
				 
				if (transferProject is Domain.TransferProjectAggregate.TransferProject transferMatProject)
				{
					transferProjectRepository.Update(transferMatProject);
				}
				await transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				completeTransmissionLogRepository.Insert(CompleteTransmissionLog.CreateTransferProjectLog(transferProject.Id, transferringAcademy.Id, completeProjectId, response.IsSuccessStatusCode, responseMessage, dateTimeProvider.Now));
				await completeTransmissionLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			}

			return new CommandSuccessResult();
		}

	}
}
