namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public record SchoolDetails(
	int Urn,
	string SchoolName,
	LandAndBuildings? LandAndBuildings,
	// additional information - split out
	Performance? Performance,
	LocalAuthority? LocalAuthority,
	PartnershipsAndAffliations? PartnershipsAndAffliations,
	ReligiousEducation? ReligiousEducation,
	// finances
	FinancialYear? PreviousFinancialYear,
	FinancialYear? CurrentFinancialYear,
	FinancialYear? NextFinancialYear,
	string? SchoolContributionToTrust = null,
	string? GoverningBodyConsentEvidenceDocumentLink = null,
	bool? AdditionalInformationAdded = null,
	string? AdditionalInformation = null,
	EqualityImpact? EqualitiesImpactAssessmentCompleted = null,
	string? EqualitiesImpactAssessmentDetails = null, // there is no text input within the UI, just a radio with the enum values
	// contact details
	string? ContactHeadName = null,
	string? ContactHeadEmail = null,
	string? ContactHeadTel = null,
	string? ContactChairName = null,
	string? ContactChairEmail = null,
	string? ContactChairTel = null,
	string? ContactRole = null, // "headteacher", "chair of governing body", "someone else"
	string? MainContactOtherName = null,
	string? MainContactOtherEmail = null,
	string? MainContactOtherTelephone = null,
	string? MainContactOtherRole = null,
	string? ApproverContactName = null,
	string? ApproverContactEmail = null,
	// conversion details
	bool? ConversionTargetDateSpecified = null,
	DateTime? ConversionTargetDate = null,
	string? ConversionTargetDateExplained = null,
	bool? ConversionChangeNamePlanned = null,
	string? ProposedNewSchoolName = null,
	string? ApplicationJoinTrustReason = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? CapacityAssumptions = null,
	int? CapacityPublishedAdmissionsNumber = null,
	// application pre-support grant
	PayFundsTo? SchoolSupportGrantFundsPaidTo = null,
	bool? ConfirmPaySupportGrantToSchool = null,
	// consultation details
	bool? SchoolHasConsultedStakeholders = null,
	string? SchoolPlanToConsultStakeholders = null,
	// Finances Investigations
	bool? FinanceOngoingInvestigations = null,
	string? FinancialInvestigationsExplain = null,
	bool? FinancialInvestigationsTrustAware = null
// TODO:- declaration
);
