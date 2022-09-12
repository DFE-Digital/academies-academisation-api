using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class PartnershipsAndAffliationsModelMapper
	{
		internal static PartnershipsAndAffliationsServiceModel ToServiceModel(this PartnershipsAndAffliations partnershipsAndAffliationsModel)
		{
			return new PartnershipsAndAffliationsServiceModel
			{
				IsPartOfFederation = partnershipsAndAffliationsModel.IsPartOfFederation,
				IsSupportedByFoundation = partnershipsAndAffliationsModel.IsSupportedByFoundation,
				SupportedFoundationName = partnershipsAndAffliationsModel.SupportedFoundationName,
				SupportedFoundationEvidenceDocumentLink = partnershipsAndAffliationsModel.SupportedFoundationEvidenceDocumentLink,
				FeederSchools = partnershipsAndAffliationsModel.FeederSchools
			};
		}

		internal static PartnershipsAndAffliations ToDomain(this PartnershipsAndAffliationsServiceModel serviceModel)
		{
			return new PartnershipsAndAffliations
			{
				IsPartOfFederation = serviceModel.IsPartOfFederation,
				IsSupportedByFoundation = serviceModel.IsSupportedByFoundation,
				SupportedFoundationName = serviceModel.SupportedFoundationName,
				SupportedFoundationEvidenceDocumentLink = serviceModel.SupportedFoundationEvidenceDocumentLink,
				FeederSchools = serviceModel.FeederSchools
			};
		}
	}
}
