using System.Net;
using System.Net.Http.Json;
using System.Text;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;
using Dfe.Academisation.CorrelationIdMiddleware;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateConversionsCompleteProjectsCommandHandler : IRequestHandler<CreateConversionsCompleteProjectsCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
		private readonly IProjectGroupRepository _projectGroupRepository;
		private readonly ICompleteTransmissionLogRepository _completeTransmissionLogRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ICompleteApiClientFactory _completeApiClientFactory;
		private readonly ILogger<CreateConversionsCompleteProjectsCommandHandler> _logger;
		private ICorrelationContext _correlationContext;

		public CreateConversionsCompleteProjectsCommandHandler(
			IConversionProjectRepository conversionProjectRepository,
			IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
			IProjectGroupRepository projectGroupRepository,
			ICompleteTransmissionLogRepository completeTransmissionLogRepository,
			ICompleteApiClientFactory completeApiClientFactory,
			IDateTimeProvider dateTimeProvider,
			ICorrelationContext correlationContext,
			ILogger<CreateConversionsCompleteProjectsCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
			_projectGroupRepository = projectGroupRepository;
			_completeTransmissionLogRepository = completeTransmissionLogRepository;
			_completeApiClientFactory = completeApiClientFactory;
			_dateTimeProvider = dateTimeProvider;
			_correlationContext = correlationContext;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(CreateConversionsCompleteProjectsCommand request,
			CancellationToken cancellationToken)
		{
			var client = _completeApiClientFactory.Create(_correlationContext);
			var retryPolicy = Policy.Handle<HttpRequestException>() // Handle HttpRequestException
									.OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && r.StatusCode != HttpStatusCode.BadRequest) // Retry if response is not successful
									.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
			(exception, timeSpan, retryCount, context) =>
			{
				_logger.LogInformation($"Retry {retryCount} after {timeSpan.Seconds} seconds due to: {exception?.Exception?.Message}");
			});

			var conversionProjects = await _conversionProjectRepository.GetProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if (conversionProjects == null || !conversionProjects.Any())
			{
				_logger.LogInformation($"No conversion projects found.");
				return new NotFoundCommandResult();
			}

			foreach (var conversionProject in conversionProjects)
			{

				var decision = await _advisoryBoardDecisionRepository.GetConversionProjectDecsion(conversionProject.Id);

				string? groupReferenceNumber = null;

				if (conversionProject.ProjectGroupId != null)
				{
					var group = await _projectGroupRepository.GetById((int)conversionProject.ProjectGroupId);
					groupReferenceNumber = group.ReferenceNumber;
				}

				var completeObject = CompleteProjectsServiceModelMapper.FromDomain(conversionProject, decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails, groupReferenceNumber);

				var response = await retryPolicy.ExecuteAsync(() => client.PostAsJsonAsync($"projects/conversions", completeObject, cancellationToken));
				Guid? completeProjectId = null;
				var responseMessage = string.Empty;

				if (response.IsSuccessStatusCode)
				{
					var successResponse = await response.Content.ReadFromJsonAsync<CreateCompleteProjectSuccessResponse>();
					completeProjectId = successResponse.conversion_project_id;

					_logger.LogInformation("Success completing conversion project with project urn: {project} with Status code 201 ", completeObject.urn);
				}
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<CreateCompleteProjectErrorResponse>();
					responseMessage = errorResponse.GetAllErrors();
					_logger.LogInformation("Error In completing conversion project with project urn: {project} due to Status code {code} and Complete Validation Errors:" + responseMessage, completeObject.urn, response.StatusCode);
				}

				conversionProject.SetProjectSentToComplete();
				_conversionProjectRepository.Update(conversionProject as Domain.ProjectAggregate.Project);
				await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				_completeTransmissionLogRepository.Insert(CompleteTransmissionLog.CreateConversionProjectLog(conversionProject.Id, completeProjectId, response.IsSuccessStatusCode, responseMessage, _dateTimeProvider.Now));
				await _completeTransmissionLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			}

			return new CommandSuccessResult();
		}
	}
}
