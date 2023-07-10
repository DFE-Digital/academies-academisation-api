using AutoMapper;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application;

internal static class ApplicationServiceModelMapper
{
	internal static ApplicationServiceModel MapFromDomain(this IApplication application, IMapper mapper)
	{
		return new(
			application.ApplicationId,
			application.ApplicationType,
			application.ApplicationStatus,
			application.Contributors
				.Select(ApplicationContributorServiceModelMapper.FromDomain).ToList(),
			application.Schools
				.Select(ApplicationSchoolServiceModelMapper.FromDomain).ToList(),
			mapper.Map<ApplicationJoinTrustServiceModel>(application.JoinTrust),
			mapper.Map<ApplicationFormTrustServiceModel>(application.FormTrust),
			application.ApplicationSubmittedDate,
			application.ApplicationReference,
			application.EntityId,
			application.Version);
	}
}
