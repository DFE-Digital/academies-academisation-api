using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class ApplicationServiceModelMapper
{
	internal static ApplicationServiceModel MapFromDomain(this IConversionApplication conversionApplication)
	{
		return new()
		{
			ApplicationId = conversionApplication.ApplicationId,
			ApplicationType = conversionApplication.ApplicationType,
			ApplicationStatus = conversionApplication.ApplicationStatus,
			Contributors = conversionApplication.Contributors
				.Select(ApplicationContributorServiceModelMapper.FromDomain).ToList(),
			Schools = conversionApplication.Schools
				.Select(ApplicationSchoolServiceModelMapper.FromDomain).ToList()
		};
	}
}
