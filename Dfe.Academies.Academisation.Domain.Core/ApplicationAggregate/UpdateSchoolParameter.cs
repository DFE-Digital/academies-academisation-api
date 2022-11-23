using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record UpdateSchoolParameter(
		int Id,
		string? TrustBenefitDetails, 
		string? OfstedInspectionDetails, 
		string? SafeguardingDetails, 
		string? LocalAuthorityReorganisationDetails,
		string? LocalAuthorityClosurePlanDetails,
		string? DioceseName,
		string? DioceseFolderIdentifier,
		bool? PartOfFederation,
		string? FoundationTrustOrBodyName,
		string? FoundationConsentFolderIdentifier,
		DateTimeOffset? ExemptionEndDate,
		string? MainFeederSchools,
		string? ResolutionConsentFolderIdentifier,
		SchoolEqualitiesProtectedCharacteristics? ProtectedCharacteristics,
		string? FurtherInformation,
		SchoolDetails SchoolDetails,
		ICollection<KeyValuePair<int, LoanDetails>> Loans,
		ICollection<KeyValuePair<int, LeaseDetails>> Leases, 
		bool? HasLoans, bool? HasLeases);
}
