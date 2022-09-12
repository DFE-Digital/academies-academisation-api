using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class LocalAuthorityModelMapper
	{
		internal static LocalAuthorityServiceModel ToServiceModel(this LocalAuthority localAuthority)
		{
			return new LocalAuthorityServiceModel
			{
				PartOfLaReorganizationPlan = localAuthority.PartOfLaReorganizationPlan,
				LaReorganizationDetails = localAuthority.LaReorganizationDetails,
				PartOfLaClosurePlan = localAuthority.PartOfLaClosurePlan,
				LaClosurePlanDetails = localAuthority.LaClosurePlanDetails
			};
		}

		internal static LocalAuthority ToDomain(this LocalAuthorityServiceModel serviceModel)
		{
			return new LocalAuthority
			{
				PartOfLaReorganizationPlan = serviceModel.PartOfLaReorganizationPlan,
				LaReorganizationDetails = serviceModel.LaReorganizationDetails,
				PartOfLaClosurePlan = serviceModel.PartOfLaClosurePlan,
				LaClosurePlanDetails = serviceModel.LaClosurePlanDetails
			};
		}
	}
}
