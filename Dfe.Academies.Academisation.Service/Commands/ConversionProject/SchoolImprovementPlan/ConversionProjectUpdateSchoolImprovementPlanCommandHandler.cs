using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan
{
	public class ConversionProjectUpdateSchoolImprovementPlanCommandHandler : IRequestHandler<ConversionProjectUpdateSchoolImprovementPlanCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly ILogger<ConversionProjectUpdateSchoolImprovementPlanCommandHandler> _logger;

		public ConversionProjectUpdateSchoolImprovementPlanCommandHandler(IConversionProjectRepository conversionProjectRepository, ILogger<ConversionProjectUpdateSchoolImprovementPlanCommandHandler> logger)
		{
			_conversionProjectRepository = conversionProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(ConversionProjectUpdateSchoolImprovementPlanCommand message, CancellationToken cancellationToken)
		{
			IProject? project = await _conversionProjectRepository.GetConversionProject(message.ProjectId, cancellationToken);

			if (project is null)
			{
				_logger.LogError($"conversion project not found with id:{message.ProjectId}");
				return new NotFoundCommandResult();
			}

			project.UpdateSchoolImprovementPlan(message.Id, message.ArrangedBy, message.ArrangedByOther, message.ProvidedBy, message.StartDate, message.ExpectedEndDate, message.ExpectedEndDateOther, message.ConfidenceLevel, message.PlanComments);

			_conversionProjectRepository.Update(project as Project);

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();

		}
	}
}
