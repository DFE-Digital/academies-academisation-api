using Azure.Core;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan
{
	public class ConversionProjectAddSchoolImprovementPlanCommandHandler : IRequestHandler<ConversionProjectAddSchoolImprovementPlanCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly ILogger<ConversionProjectAddSchoolImprovementPlanCommandHandler> _logger;

		public ConversionProjectAddSchoolImprovementPlanCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<ConversionProjectAddSchoolImprovementPlanCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(ConversionProjectAddSchoolImprovementPlanCommand message, CancellationToken cancellationToken)
		{
			IProject? project = await _conversionProjectRepository.GetConversionProject(message.ProjectId, cancellationToken);

			if (project is null)
			{
				_logger.LogError($"conversion project not found with id:{message.ProjectId}");
				return new NotFoundCommandResult();
			}

			project.AddSchoolImprovementPlan(message.ArrangedBy, message.ArrangedByOther, message.ProvidedBy, message.StartDate, message.ExpectedEndDate, message.ExpectedEndDateOther, message.ConfidenceLevel, message.PlanComments);

			_conversionProjectRepository.Update((Project)project);

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();

		}
	}
}
