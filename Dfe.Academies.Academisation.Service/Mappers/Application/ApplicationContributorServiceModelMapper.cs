using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class ApplicationContributorServiceModelMapper
{
	internal static ApplicationContributorServiceModel FromDomain(IContributor contributor)
	{
		return new(
			contributor.Id,
			contributor.Details.FirstName,
			contributor.Details.LastName,
			contributor.Details.EmailAddress,
			contributor.Details.Role,
			contributor.Details.OtherRoleName
		);
	}

	internal static ContributorDetails ToDomain(this ApplicationContributorServiceModel serviceModel)
	{
		return new(
			serviceModel.FirstName,
			serviceModel.LastName,
			serviceModel.EmailAddress,
			serviceModel.Role,
			serviceModel.OtherRoleName,
			null
		);
	}
}
