using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record ApplicationSchoolServiceModel(
	int Id,
	int Urn,
	string SchoolName,
	LandAndBuildingsServiceModel? LandAndBuildings,
	// additional information - split up
	
	
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
	
	// finances
	FinancialYearServiceModel? PreviousFinancialYear,
	FinancialYearServiceModel? CurrentFinancialYear,
	FinancialYearServiceModel? NextFinancialYear,
	// leases & loans
	IReadOnlyCollection<LoanServiceModel> Loans,
	IReadOnlyCollection<LeaseServiceModel> Leases,
	string? SchoolContributionToTrust = null,
	string? GoverningBodyConsentEvidenceDocumentLink = null,
	bool? AdditionalInformationAdded = null,
	string? AdditionalInformation = null,
	EqualityImpact? EqualitiesImpactAssessmentCompleted = null,
	string? EqualitiesImpactAssessmentDetails = null, // two possible very long proforma string? answers here - maybe this should be a bool
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
	// conversion details
	bool? SchoolConversionTargetDateSpecified = null,
	DateTime? SchoolConversionTargetDate = null,
	string? SchoolConversionTargetDateExplained = null,
	bool? ConversionChangeNamePlanned = null,
	string? ProposedNewSchoolName = null,
	string? ApplicationJoinTrustReason = null,
	// future pupil numbers
	int? ProjectedPupilNumbersYear1 = null,
	int? ProjectedPupilNumbersYear2 = null,
	int? ProjectedPupilNumbersYear3 = null,
	string? SchoolCapacityAssumptions = null,
	int? SchoolCapacityPublishedAdmissionsNumber = null,
	// application pre-support grant
	PayFundsTo? SchoolSupportGrantFundsPaidTo = null,
	bool? ConfirmPaySupportGrantToSchool = null,
	// consultation details
	bool? SchoolHasConsultedStakeholders = null,
	string? SchoolPlanToConsultStakeholders = null,
	// Finances Investigations
	bool? FinanceOngoingInvestigations = null,
	string? FinancialInvestigationsExplain = null,
	bool? FinancialInvestigationsTrustAware = null,
	// Declaration
	bool? DeclarationBodyAgree = null,
	bool? DeclarationIAmTheChairOrHeadteacher = null,
	string? DeclarationSignedByName = null,
	string? SchoolConversionReasonsForJoining = null, 
	bool? HasLoans = null,
	bool? HasLeases = null,
	Guid? EntityId = null
);
