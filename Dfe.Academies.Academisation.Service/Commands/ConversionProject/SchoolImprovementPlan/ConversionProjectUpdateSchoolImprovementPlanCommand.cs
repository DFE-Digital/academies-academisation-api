using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan
{
	public record ConversionProjectUpdateSchoolImprovementPlanCommand(
		int Id,
		int ProjectId,
		List<SchoolImprovementPlanArranger> ArrangedBy,
		string? ArrangedByOther,
		string ProvidedBy,
		DateTime StartDate,
		SchoolImprovementPlanExpectedEndDate ExpectedEndDate,
		DateTime? ExpectedEndDateOther,
		SchoolImprovementPlanConfidenceLevel ConfidenceLevel,
		string? PlanComments) : IRequest<CommandResult>;
}
