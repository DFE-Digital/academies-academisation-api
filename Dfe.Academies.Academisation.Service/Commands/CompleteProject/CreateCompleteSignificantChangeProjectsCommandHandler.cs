using System.Net.Http.Json;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
    public class CreateCompleteSignificantChangeProjectsCommandHandler(
        ICompleteApiClientRetryFactory completeApiClientRetryFactory,
		ISignificantChangeProjectRepository significantChangeProjectRepository,
		IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
		ICompleteTransmissionLogRepository completeTransmissionLogRepository,
		IDateTimeProvider dateTimeProvider,
		ILogger<CreateCompleteSignificantChangeProjectsCommandHandler> logger)
        : IRequestHandler<CreateCompleteSignificantChangeProjectsCommand, CommandResult>
    {
        public async Task<CommandResult> Handle(CreateCompleteSignificantChangeProjectsCommand request,
            CancellationToken cancellationToken)
        {
            var retryPolicy = completeApiClientRetryFactory.GetCompleteHttpClientRetryPolicy(logger);

            var significantChangeProjects = await significantChangeProjectRepository.GetProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

            if (significantChangeProjects.Count == 0)
            {
                logger.LogInformation("No significant change projects found.");
                return new NotFoundCommandResult();
            }

			foreach (var significantChangeProject in significantChangeProjects)
			{
				var decision = await advisoryBoardDecisionRepository.GetSignificantChangeDecision(significantChangeProject.Id);

				var completeObject = CompleteConversionProjectServiceModelMapper.FromDomain(significantChangeProject, decision);

				var response = await completeApiClientRetryFactory.CreateSignificantChangeProjectAsync(completeObject, retryPolicy, cancellationToken);

				Guid? completeProjectId = null;
				string responseMessage = string.Empty;

				if (response.IsSuccessStatusCode)
				{
					var successResponse = await response.Content.ReadFromJsonAsync<CreateCompleteSignificantChangeSuccessResponse>(cancellationToken);
					completeProjectId = successResponse?.significantchange_project_id;

					logger.LogInformation("Success sending significant change project to complete with project urn: {AcademyUrn} with Status code 201 ", completeObject.AcademyUrn);
				}
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<CreateCompleteProjectErrorResponse>(cancellationToken);
					responseMessage = errorResponse?.Response ?? "No error message returned";
					logger.LogError("Error sending significant change project to complete with project urn: {AcademyUrn} due to Status code {StatusCode} and Complete Validation Errors: {ResponseMessage}", completeObject.AcademyUrn, response.StatusCode, responseMessage);
				}

				significantChangeProject.SetProjectSentToComplete(completeProjectId);

				await significantChangeProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				completeTransmissionLogRepository.Insert(CompleteTransmissionLog.CreateSignificantChangeProjectLog(significantChangeProject.Id, completeProjectId, response.IsSuccessStatusCode, responseMessage, dateTimeProvider.Now));
				await completeTransmissionLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			}
			return new CommandSuccessResult();
		}

    }
}
