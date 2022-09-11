namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record LocalAuthorityServiceModel(
		bool? PartOfLaReorganizationPlan = null,
		string? LaReorganizationDetails = null,
		bool? PartOfLaClosurePlan = null,
		string? LaClosurePlanDetails = null
		);
}
