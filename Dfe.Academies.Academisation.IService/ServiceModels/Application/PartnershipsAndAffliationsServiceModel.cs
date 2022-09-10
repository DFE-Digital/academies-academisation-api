namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record PartnershipsAndAffliationsServiceModel(
		bool? IsPartOfFederation = null,
		bool? IsSupportedByFoundation = null,
		string? SupportedFoundationBodyName = null,
		string? SupportedFoundationEvidenceDocumentLink = null,
		string? FeederSchools = null // just detail the top 5 - free text - don't select school Urn via UI
		);
}
