namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public record SchoolDetails(
	int Urn,
	string SchoolName,
	LandAndBuildings LandAndBuildings,
	// additional information - split out
	Performance Performance,
	PartnershipsAndAffliations PartnershipsAndAffliations,
	ReligiousEducation ReligiousEducation,
	// TODO MR:- UI uses an enum A2CEqualityImpact
	bool? EqualitiesImpactAssessmentCompleted = null,
	string? EqualitiesImpactAssessmentDetails = null, // two possible very long proforma string? answers here - maybe this should be a bool
	// TODO MR:- Local auth questions in UI
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
	bool? ConfirmPaySupportGrantToSchool = null
);
