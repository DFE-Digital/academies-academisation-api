using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan
{
	public class ConversionProjectAddSchoolImprovementPlanCommandHandler : IRequestHandler<ConversionProjectAddSchoolImprovementPlanCommand, CommandResult>
	{
		private readonly IConversionProjectRepository _conversionProjectRepository;


		public ConversionProjectAddSchoolImprovementPlanCommandHandler(IConversionProjectRepository conversionProjectRepository)
		{
			_conversionProjectRepository = conversionProjectRepository;
		}

		public async Task<CommandResult> Handle(ConversionProjectAddSchoolImprovementPlanCommand message, CancellationToken cancellationToken)
		{
			IProject? project = await _conversionProjectRepository.GetConversionProject(message.ProjectId);

			if (project is null)
			{
				return new NotFoundCommandResult();
			}

			project.AddSchoolImprovementPlan(message.ArrangedBy, message.ArrangedByOther, message.ProvidedBy, message.StartDate, message.ExpectedEndDate, message.ExpectedEndDateOther, message.ConfidenceLevel, message.PlanComments);

			await _conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();

		}
	}
}
