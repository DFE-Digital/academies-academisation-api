using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class PartnershipsAndAffliationsModelMapper
	{
		internal static PartnershipsAndAffliationsServiceModel ToServiceModel(this PartnershipsAndAffliations partnershipsAndAffliationsModel)
		{
			//TODO MR:-
			//bool? IsPartOfFederation = null,
			//bool? IsSupportedByFoundation = null,
			//string? SupportedFoundationBodyName = null,
			//string? SupportedFoundationEvidenceDocumentLink = null,
			//string? FeederSchools = null // just detail the top 5 - free text - don't select school Urn via UI
		}

		internal static PartnershipsAndAffliations ToDomain(this PartnershipsAndAffliationsServiceModel serviceModel)
		{
			//TODO MR:-
		}
	}
}
