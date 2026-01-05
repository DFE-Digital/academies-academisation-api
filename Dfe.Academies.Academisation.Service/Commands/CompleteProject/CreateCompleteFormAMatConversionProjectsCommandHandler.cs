using System.Net.Http.Json;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateCompleteFormAMatConversionProjectsCommandHandler(
		IConversionProjectRepository conversionProjectRepository,
		IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
		IProjectGroupRepository projectGroupRepository,
		ICompleteTransmissionLogRepository completeTransmissionLogRepository, 
		IDateTimeProvider dateTimeProvider,
		ICompleteApiClientRetryFactory completeApiClientRetryFactory,
		ILogger<CreateCompleteFormAMatConversionProjectsCommandHandler> logger) : IRequestHandler<CreateCompleteFormAMatConversionProjectsCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(CreateCompleteFormAMatConversionProjectsCommand request,
			CancellationToken cancellationToken)
		{

			var conversionProjects = await conversionProjectRepository.GetFormAMatProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if (conversionProjects == null || !conversionProjects.Any())
			{
				logger.LogInformation($"No conversion projects found.");
				return new NotFoundCommandResult();
			}
			var retryPolicy = completeApiClientRetryFactory.GetCompleteHttpClientRetryPolicy(logger);
			foreach (var conversionProject in conversionProjects)
			{ 
				var decision = await advisoryBoardDecisionRepository.GetConversionProjectDecsion(conversionProject.Id);

				string? groupReferenceNumber = null;

				if (conversionProject.ProjectGroupId != null)
				{
					var group = await projectGroupRepository.GetById((int)conversionProject.ProjectGroupId);
					groupReferenceNumber = group.ReferenceNumber;
				}

				var completeObject = CompleteConversionProjectServiceModelMapper.FormAMatFromDomain(conversionProject, decision?.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails!, groupReferenceNumber!);
 
				var response = await completeApiClientRetryFactory.CreateConversionMatProjectAsync(
						completeObject, retryPolicy, cancellationToken);

				Guid? completeProjectId = null;
				string? responseMessage = string.Empty;

				if (response.IsSuccessStatusCode)
				{
					var successResponse = await response.Content.ReadFromJsonAsync<CreateCompleteConversionProjectSuccessResponse>(cancellationToken);
					completeProjectId = successResponse?.conversion_project_id;

					logger.LogInformation("Success sending conversion project to complete with project urn: {Urn} with Status code 201 ", completeObject.Urn);
				}
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<CreateCompleteProjectErrorResponse>(cancellationToken);
					responseMessage = errorResponse?.Response ?? "No error message returned";
					logger.LogError("Error sending conversion project to complete with project urn: {Urn} due to Status code {Code} and Complete Validation Errors: {ResponseMessage}", completeObject.Urn, response.StatusCode, responseMessage);
				}

				conversionProject.SetProjectSentToComplete(); 
				if (conversionProject is Domain.ProjectAggregate.Project conversionMatProject)
				{
					conversionProjectRepository.Update(conversionMatProject);
				}
				await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				completeTransmissionLogRepository.Insert(CompleteTransmissionLog.CreateConversionProjectLog(conversionProject.Id, completeProjectId, response.IsSuccessStatusCode, responseMessage!, dateTimeProvider.Now));
				await completeTransmissionLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			}

			return new CommandSuccessResult();
		}
	}
}
