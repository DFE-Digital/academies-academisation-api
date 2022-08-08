using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

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
}
