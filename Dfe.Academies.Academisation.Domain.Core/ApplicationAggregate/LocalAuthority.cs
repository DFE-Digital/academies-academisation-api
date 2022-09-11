namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record LocalAuthority(
		bool? PartOfLaReorganizationPlan = null,
		string? LaReorganizationDetails = null,
		bool? PartOfLaClosurePlan = null,
		string? LaClosurePlanDetails = null
		);
}
