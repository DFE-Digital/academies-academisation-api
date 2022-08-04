using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationServiceModelMapper
{
	internal static ApplicationServiceModel MapFromDomain(this IConversionApplication conversionApplication)
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
