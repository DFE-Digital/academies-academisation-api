namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

public record LegacySchoolServiceModel(
	ICollection<LegacyLoanServiceModel> SchoolLoans,
	ICollection<LegacyLeaseServiceModel> SchoolLeases,
	string? SchoolName = null,
	// contact details
	string? SchoolConversionContactHeadName = null,
	string? SchoolConversionContactHeadEmail = null,
	string? SchoolConversionContactHeadTel = null,
	string? SchoolConversionContactChairName = null,
	string? SchoolConversionContactChairEmail = null,
	string? SchoolConversionContactChairTel = null,
	string? SchoolConversionContactRole = null, // "headteacher", "chair of governing body", "someone else"
	string? SchoolConversionMainContactOtherName = null,
	string? SchoolConversionMainContactOtherEmail = null,
	string? SchoolConversionMainContactOtherTelephone = null,
	string? SchoolConversionMainContactOtherRole = null,
	string? SchoolConversionApproverContactName = null,
	string? SchoolConversionApproverContactEmail = null,
	// conversion dates
	bool? SchoolConversionTargetDateSpecified = null,
	DateTime? SchoolConversionTargetDate = null,
	string? SchoolConversionTargetDateExplained = null,
	// reasons for joining
	string? SchoolConversionReasonsForJoining = null,
	// name changes
	bool? SchoolConversionChangeNamePlanned = null,
	string? SchoolConversionProposedNewSchoolName = null,
	// additional information
	string? SchoolAdSchoolContributionToTrust = null,
	bool? SchoolOngoingSafeguardingInvestigations = null,
	string? SchoolOngoingSafeguardingDetails = null,
	bool? SchoolPartOfLaReorganizationPlan = null,
	string? SchoolLaReorganizationDetails = null,
	bool? SchoolPartOfLaClosurePlan = null,
	string? SchoolLaClosurePlanDetails = null,
	bool? SchoolFaithSchool = null,
	string? SchoolFaithSchoolDioceseName = null,
	string? DiocesePermissionEvidenceDocumentLink = null,
	bool? SchoolIsPartOfFederation = null,
	bool? SchoolIsSupportedByFoundation = null,
	string? SchoolSupportedFoundationBodyName = null,
	string? FoundationEvidenceDocumentLink = null,
	bool? SchoolHasSACREException = null,
	DateTime? SchoolSACREExemptionEndDate = null,
	string? SchoolAdFeederSchools = null,
	string? GoverningBodyConsentEvidenceDocumentLink = null,
	bool? SchoolAdEqualitiesImpactAssessmentCompleted = null,
	string? SchoolAdEqualitiesImpactAssessmentDetails = null, // two possible very long proforma string? answers here - maybe this should be a bool
	bool? SchoolAdInspectedButReportNotPublished = null,
	string? SchoolAdInspectedButReportNotPublishedExplain = null,
	bool? SchoolAdditionalInformationAdded = null,
	string? SchoolAdditionalInformation = null,
	// Finances
	LegacyFinancialYearServiceModel? PreviousFinancialYear = null,
	LegacyFinancialYearServiceModel? CurrentFinancialYear = null,
	LegacyFinancialYearServiceModel? NextFinancialYear = null,
	bool? FinanceOngoingInvestigations = null,
	string? SchoolFinancialInvestigationsExplain = null,
	bool? SchoolFinancialInvestigationsTrustAware = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? SchoolCapacityAssumptions = null,
	int? SchoolCapacityPublishedAdmissionsNumber = null,
	// land and buildings
	string? SchoolBuildLandOwnerExplained = null,
	bool? SchoolBuildLandWorksPlanned = null,
	string? SchoolBuildLandWorksPlannedExplained = null,
	DateTime? SchoolBuildLandWorksPlannedCompletionDate = null,
	bool? SchoolBuildLandSharedFacilities = null,
	string? SchoolBuildLandSharedFacilitiesExplained = null,
	bool? SchoolBuildLandGrants = null,
	string? SchoolBuildLandGrantsExplained = null,
	bool? SchoolBuildLandPFIScheme = null,
	string? SchoolBuildLandPFISchemeType = null,
	bool? SchoolBuildLandPriorityBuildingProgramme = null,
	bool? SchoolBuildLandFutureProgramme = null,
	// pre-opening support grant
	string? SchoolSupportGrantFundsPaidTo = null, // either "To the school" or "To the trust the school is joining"
	// consultation details
	bool? SchoolHasConsultedStakeholders = null,
	string? SchoolPlanToConsultStakeholders = null,
	// declaration
	// two questions from the application form would be easy to mix up here
	// 1. I agree with all of these statements, and belive that the facts stated in this application are true (summary page)
	// 2. The information in this application is true to the best of my kowledge (actual question)
	bool? DeclarationBodyAgree = null,
	bool? DeclarationIAmTheChairOrHeadteacher = null,
	string? DeclarationSignedByName = null
);
