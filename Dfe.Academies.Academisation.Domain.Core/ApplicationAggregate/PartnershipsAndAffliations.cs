﻿namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record PartnershipsAndAffliations(
		bool? IsPartOfFederation = null,
		bool? IsSupportedByFoundation = null,
		string? SupportedFoundationBodyName = null,
		string? FoundationEvidenceDocumentLink = null,
		string? FeederSchools = null
		);
}
