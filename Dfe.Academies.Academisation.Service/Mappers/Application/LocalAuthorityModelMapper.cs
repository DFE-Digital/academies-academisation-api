using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class LocalAuthorityModelMapper
	{
		internal static LocalAuthorityServiceModel ToServiceModel(this LocalAuthority localAuthority)
		{
			// TODO MR:-
			//bool? PartOfLaReorganizationPlan = null,
			//string? LaReorganizationDetails = null,
			//bool? PartOfLaClosurePlan = null,
			//string? LaClosurePlanDetails = null
		}

		internal static LocalAuthority ToDomain(this LocalAuthorityServiceModel serviceModel)
		{
			// TODO MR:-
			//bool? PartOfLaReorganizationPlan = null,
			//string? LaReorganizationDetails = null,
			//bool? PartOfLaClosurePlan = null,
			//string? LaClosurePlanDetails = null
		}
	}
}
