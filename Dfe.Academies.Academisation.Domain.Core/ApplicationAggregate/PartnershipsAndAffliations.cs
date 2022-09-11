namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record PartnershipsAndAffliations(
		bool? IsPartOfFederation = null,
		bool? IsSupportedByFoundation = null,
		string? SupportedFoundationName = null,
		string? SupportedFoundationEvidenceDocumentLink = null,
		string? FeederSchools = null // just detail the top 5 - free text - don't select school Urn via UI
		);
}
