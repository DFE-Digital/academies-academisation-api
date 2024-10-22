using System.Net.Http.Json;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Complete;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.CompleteProjects;
using Dfe.Academisation.CorrelationIdMiddleware;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateCompleteFormAMatTransferProjectsCommandHandler : IRequestHandler<CreateCompleteFormAMatTransferProjectsCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
		private readonly IProjectGroupRepository _projectGroupRepository;
		private readonly ICompleteTransmissionLogRepository _completeTransmissionLogRepository;
		private readonly IAcademiesQueryService _academiesQueryService;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ICompleteApiClientFactory _completeApiClientFactory;
		private readonly ILogger<CreateCompleteFormAMatTransferProjectsCommandHandler> _logger;
		private ICorrelationContext _correlationContext;
		private readonly IPollyPolicyFactory _pollyPolicyFactory;

		public CreateCompleteFormAMatTransferProjectsCommandHandler(
			ITransferProjectRepository transferProjectRepository,
			IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
			IAcademiesQueryService academiesQueryService,
			IProjectGroupRepository projectGroupRepository,
			ICompleteTransmissionLogRepository completeTransmissionLogRepository,
			ICompleteApiClientFactory completeApiClientFactory,
			IDateTimeProvider dateTimeProvider,
			ICorrelationContext correlationContext,
			IPollyPolicyFactory pollyPolicyFactory,
			ILogger<CreateCompleteFormAMatTransferProjectsCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
			_academiesQueryService = academiesQueryService;
			_projectGroupRepository = projectGroupRepository;
			_completeTransmissionLogRepository = completeTransmissionLogRepository;
			_completeApiClientFactory = completeApiClientFactory;
			_dateTimeProvider = dateTimeProvider;
			_correlationContext = correlationContext;
			_pollyPolicyFactory = pollyPolicyFactory;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(CreateCompleteFormAMatTransferProjectsCommand request,
			CancellationToken cancellationToken)
		{
			var client = _completeApiClientFactory.Create(_correlationContext);
			var retryPolicy = _pollyPolicyFactory.GetCompleteHttpClientRetryPolicy(_logger);

			var transferProjects = await _transferProjectRepository.GetFormAMatProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if (transferProjects == null || !transferProjects.Any())
			{
				_logger.LogInformation($"No transfer projects found.");
				return new NotFoundCommandResult();
			}

			var transferingAcademies = transferProjects.SelectMany(x => x.TransferringAcademies,
				(transferProject, academy) => new
				{
					academy.Id,
					TransferProjectId = transferProject.Id,
					Ukprn = academy.OutgoingAcademyUkprn,
					Include = !academy.ProjectSentToComplete
				}).Where(x => x.Include == true);

			var establishments = await _academiesQueryService.GetBulkEstablishmentsByUkprn(transferingAcademies.Select(x => x.Ukprn));

			foreach (var transferringAcademy in transferingAcademies)
			{
				var transferProject = transferProjects.Single(x => x.Id == transferringAcademy.TransferProjectId);
				var decision = await _advisoryBoardDecisionRepository.GetTransferProjectDecsion(transferringAcademy.TransferProjectId);
				var establishment = establishments.Single(x => x.Ukprn == transferringAcademy.Ukprn);

				var transferObject = CompleteTransferProjectServiceModelMapper.FormAMatFromDomain(transferProject,
					decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails, establishment.Urn);

				var response =
					await retryPolicy.ExecuteAsync(() => client.PostAsJsonAsync($"projects/transfers/form-a-mat", transferObject, cancellationToken));

				Guid? completeProjectId = null;
				var responseMessage = string.Empty;

				if (response.IsSuccessStatusCode)
				{
					var successResponse =
						await response.Content.ReadFromJsonAsync<CreateCompleteProjectSuccessResponse>();

					completeProjectId = successResponse.conversion_project_id;

					_logger.LogInformation(
						"Success sending transfer to complete with project urn: {project} with Status code 201 ",
						transferProject.Urn);
				}
				else
				{
					var errorResponse =
						await response.Content.ReadFromJsonAsync<CreateCompleteProjectErrorResponse>();
					responseMessage = errorResponse.GetAllErrors();
					_logger.LogInformation("Error sending transfer project to complete with project urn: {project} for transfering academy: {urn} due to Status code {code} and Complete Validation Errors:" + responseMessage, transferProject.Urn, establishment.Urn, response.StatusCode);
				}

				transferProject.SetProjectSentToComplete(transferringAcademy.Ukprn);

				_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);

				await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				_completeTransmissionLogRepository.Insert(CompleteTransmissionLog.CreateTransferProjectLog(transferProject.Id, transferringAcademy.Id, completeProjectId, response.IsSuccessStatusCode, responseMessage, _dateTimeProvider.Now));
				await _completeTransmissionLogRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			}

			return new CommandSuccessResult();
		}

	}
}
