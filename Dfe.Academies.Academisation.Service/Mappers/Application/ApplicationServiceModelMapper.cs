using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

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
				.Select(ApplicationSchoolServiceModelMapper.FromDomain).ToList(),
			ApplicationTrustServiceModelMapper.FromDomain(application.JoinTrust!),
			ApplicationTrustServiceModelMapper.FromDomain(application.FormTrust!));
	}
}
