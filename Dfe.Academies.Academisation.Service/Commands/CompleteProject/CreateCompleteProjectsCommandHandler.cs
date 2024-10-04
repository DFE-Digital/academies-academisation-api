using System.Net;
using System.Net.Http.Json;
using System.Text;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
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
	public class CreateCompleteProjectsCommandHandler : IRequestHandler<CreateCompleteProjectsCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ICompleteApiClientFactory _completeApiClientFactory;
		private readonly ILogger<CreateCompleteProjectsCommandHandler> _logger;
		private ICorrelationContext _correlationContext;

		public CreateCompleteProjectsCommandHandler(
			IConversionProjectRepository conversionProjectRepository,
			IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
			ICompleteApiClientFactory completeApiClientFactory,
			IDateTimeProvider dateTimeProvider,
			ICorrelationContext correlationContext,
			ILogger<CreateCompleteProjectsCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
			_completeApiClientFactory = completeApiClientFactory;
			_dateTimeProvider = dateTimeProvider;
			_correlationContext = correlationContext;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(CreateCompleteProjectsCommand request,
			CancellationToken cancellationToken)
		{
			var client = _completeApiClientFactory.Create(_correlationContext);
			
			var conversionProjects = await _conversionProjectRepository.GetProjectsToSendToCompleteAsync(cancellationToken).ConfigureAwait(false);

			if (conversionProjects == null || !conversionProjects.Any())
			{
				_logger.LogInformation($"No conversion projects found.");
				return new NotFoundCommandResult();
			}

			foreach (var conversionProject in conversionProjects)
			{

				var decision = await _advisoryBoardDecisionRepository.GetConversionProjectDecsion(conversionProject.Id);
					
				var completeObject = CompleteProjectsServiceModelMapper.FromDomain(conversionProject, decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails);
				
				var response = await client.PostAsJsonAsync($"projects/conversions", completeObject, cancellationToken);
				
				if (response.StatusCode == HttpStatusCode.OK){
					
					var successResponse = await response.Content.ReadFromJsonAsync<CreateProjectSuccessResponse>(); ;

					conversionProject.SetCompleteProjectId(successResponse.conversion_project_id);

					_conversionProjectRepository.Update(conversionProject as Domain.ProjectAggregate.Project);
				}
			}
			
			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			
			return new CommandSuccessResult();
		}
	}
}
