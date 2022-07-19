using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationContributorServiceModelMapper
{
	internal static ApplicationContributorServiceModel FromDomain(IContributor contributor)
	{
		return new()
		{
			ContributorId = contributor.Id,
			FirstName = contributor.Details.FirstName,
			LastName = contributor.Details.LastName,
			EmailAddress = contributor.Details.EmailAddress,
			Role = contributor.Details.Role,
			OtherRoleName = contributor.Details.OtherRoleName
		};
	} 
}
