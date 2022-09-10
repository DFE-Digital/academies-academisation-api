namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record PartnershipsAndAffliationsServiceModel(
		bool? IsPartOfFederation = null,
		bool? IsSupportedByFoundation = null,
		string? SupportedFoundationBodyName = null,
		string? FoundationEvidenceDocumentLink = null,
		string? FeederSchools = null
		);
}
