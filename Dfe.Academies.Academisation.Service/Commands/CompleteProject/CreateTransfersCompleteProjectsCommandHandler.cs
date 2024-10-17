using System.Net;
using System.Net.Http.Json;
using System.Text;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
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

namespace Dfe.Academies.Academisation.Service.Commands.CompleteProject
{
	public class CreateTransfersCompleteProjectsCommandHandler : IRequestHandler<CreateTransfersCompleteProjectsCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
		private readonly IProjectGroupRepository _projectGroupRepository;
		private readonly IAcademiesQueryService _academiesQueryService;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ICompleteApiClientFactory _completeApiClientFactory;
		private readonly ILogger<CreateTransfersCompleteProjectsCommandHandler> _logger;
		private ICorrelationContext _correlationContext;

		public CreateTransfersCompleteProjectsCommandHandler(
			ITransferProjectRepository transferProjectRepository,
			IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,	
			IAcademiesQueryService academiesQueryService,
			IProjectGroupRepository projectGroupRepository,
			ICompleteApiClientFactory completeApiClientFactory,
			IDateTimeProvider dateTimeProvider,
			ICorrelationContext correlationContext,
			ILogger<CreateTransfersCompleteProjectsCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
			_academiesQueryService = academiesQueryService;
			_projectGroupRepository = projectGroupRepository;
			_completeApiClientFactory = completeApiClientFactory;
			_dateTimeProvider = dateTimeProvider;
			_correlationContext = correlationContext;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(CreateTransfersCompleteProjectsCommand request,
			CancellationToken cancellationToken)
		{
			var client = _completeApiClientFactory.Create(_correlationContext);

			
			var transferProjects = await _transferProjectRepository.GetProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if ( transferProjects == null || !transferProjects.Any())
			{
				_logger.LogInformation($"No transfer projects found.");
				return new NotFoundCommandResult();
			}

			foreach (var transferProject in transferProjects )
			{
				List<string> listOfAcademyUrns = new();
				foreach (TransferringAcademy transferingAcademy in transferProject.TransferringAcademies)
				{
					var establishment = await _academiesQueryService.GetEstablishmentByUkprn(transferingAcademy.OutgoingAcademyUkprn);
					listOfAcademyUrns.Add(establishment.Urn);
				}

				foreach (string g in listOfAcademyUrns)
				{

					var decision = await _advisoryBoardDecisionRepository.GetTransferProjectDecsion(transferProject.Id);


					var transferObject = CompleteProjectsServiceModelMapper.FromDomain(transferProject,
						decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails, g);

					var response =
						await client.PostAsJsonAsync($"projects/transfers", transferObject, cancellationToken);

					if (response.StatusCode == HttpStatusCode.Created)
					{

						var successResponse =
							await response.Content.ReadFromJsonAsync<CreateCompleteProjectSuccessResponse>();
						

						transferProject.SetCompleteProjectId(successResponse.conversion_project_id);

						_transferProjectRepository.Update(
							transferProject as Domain.TransferProjectAggregate.TransferProject);

						await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

						_logger.LogInformation(
							"Success completing conversion project with project urn: {project} with Status code 201 ",
							transferProject.Urn);
					}

					else
					{
						var errorResponse =
							await response.Content.ReadFromJsonAsync<CreateCompleteProjectErrorResponse>();
						var errorResponseMessage = errorResponse.GetAllErrors();
						_logger.LogInformation("Error In completing conversion project with project urn: {project} for transfering academy: {urn} due to Status code {code} and Complete Validation Errors:" + errorResponseMessage,transferProject.Urn, response.StatusCode);
					}

				}
			}
			
			return new CommandSuccessResult();
		}
		
	}
}
