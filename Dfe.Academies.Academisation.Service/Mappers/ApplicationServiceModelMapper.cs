using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationServiceModelMapper
{
	internal static ApplicationServiceModel MapFromDomain(this IApplication application)
	{
		return new(
			application.ApplicationId,
			application.ApplicationType,
			application.ApplicationStatus,
			application.Contributors
				.Select(ApplicationContributorServiceModelMapper.FromDomain).ToList(),
			application.Schools
				.Select(ApplicationSchoolServiceModelMapper.FromDomain).ToList());
	}
}
