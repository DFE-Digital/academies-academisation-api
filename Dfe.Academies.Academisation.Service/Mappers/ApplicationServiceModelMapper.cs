using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationServiceModelMapper
{
	internal static ApplicationServiceModel MapFromDomain(this IApplication conversionApplication)
	{
		return new(
			conversionApplication.ApplicationId,
			conversionApplication.ApplicationType,
			conversionApplication.ApplicationStatus,
			conversionApplication.Contributors
				.Select(ApplicationContributorServiceModelMapper.FromDomain).ToList(),
			conversionApplication.Schools
				.Select(ApplicationSchoolServiceModelMapper.FromDomain).ToList());
	}
}
