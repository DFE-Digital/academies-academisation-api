using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.Mappers.Legacy.ApplicationAggregate;

internal static class LegacySchoolServiceModelMapper
{
	internal static ICollection<LegacySchoolServiceModel> MapToServiceModels(this IApplication application)
	{
		ICollection<LegacySchoolServiceModel> schools = application.Schools.Select(s => MapToServiceModel(s.Details)).ToList();

		return schools;
	}

	private static LegacySchoolServiceModel MapToServiceModel(this SchoolDetails school)
	{
		LegacySchoolServiceModel serviceModel = new(
			// ToDo: Loans
			new List<LegacyLoanServiceModel>(),
			// ToDo: Leases
			new List<LegacyLeaseServiceModel>())
		{
			SchoolName = school.SchoolName,

			// contact details
			SchoolConversionContactHeadName = school.ContactHeadName,
			SchoolConversionContactHeadEmail = school.ContactHeadEmail,
			SchoolConversionContactHeadTel = school.ContactHeadTel,
			SchoolConversionContactChairName = school.ContactChairName,
			SchoolConversionContactChairEmail = school.ContactChairEmail,
			SchoolConversionContactChairTel = school.ContactChairTel,
			SchoolConversionContactRole = school.ContactRole,
			SchoolConversionMainContactOtherName = school.MainContactOtherName,
			SchoolConversionMainContactOtherEmail = school.MainContactOtherEmail,
			SchoolConversionMainContactOtherTelephone = school.MainContactOtherTelephone,
			SchoolConversionMainContactOtherRole = school.MainContactOtherRole,
			SchoolConversionApproverContactName = school.ApproverContactName,
			SchoolConversionApproverContactEmail = school.ApproverContactEmail,

			// conversion dates
			SchoolConversionTargetDateSpecified = school.SchoolConversionTargetDateSpecified.Value,
			SchoolConversionTargetDate = school.ConversionTargetDate,
			SchoolConversionTargetDateExplained = school.ConversionTargetDateExplained,

			// reasons for joining
			SchoolConversionReasonsForJoining = school.ApplicationJoinTrustReason,

			// name changes
			SchoolConversionChangeNamePlanned = school.ProposedNewSchoolName is not null,
			SchoolConversionProposedNewSchoolName = school.ProposedNewSchoolName,

			// ToDo: additional information
			////SchoolAdSchoolContributionToTrust = null,
			////SchoolOngoingSafeguardingInvestigations = null,
			////SchoolOngoingSafeguardingDetails = null,
			////SchoolPartOfLaReorganizationPlan = null,
			////SchoolLaReorganizationDetails = null,
			////SchoolPartOfLaClosurePlan = null,
			////SchoolLaClosurePlanDetails = null,
			////SchoolFaithSchool = null,
			////SchoolFaithSchoolDioceseName = null,
			////DiocesePermissionEvidenceDocumentLink = null,
			////SchoolIsPartOfFederation = null,
			////SchoolIsSupportedByFoundation = null,
			////SchoolSupportedFoundationBodyName = null,
			////FoundationEvidenceDocumentLink = null,
			////SchoolHasSACREException = null,
			////SchoolSACREExemptionEndDate = null,
			////SchoolAdFeederSchools = null,
			////GoverningBodyConsentEvidenceDocumentLink = null,
			////SchoolAdEqualitiesImpactAssessmentCompleted = null,
			////SchoolAdEqualitiesImpactAssessmentDetails = null, // two possible very long proforma string? answers here - maybe this should be a bool
			////SchoolAdInspectedButReportNotPublished = null,
			////SchoolAdInspectedButReportNotPublishedExplain = null,
			////SchoolAdditionalInformationAdded = null,
			////SchoolAdditionalInformation = null,

			// ToDo: Finances
			////PreviousFinancialYear = null,
			////CurrentFinancialYear = null,
			////NextFinancialYear = null,
			////FinanceOngoingInvestigations = null,
			////SchoolFinancialInvestigationsExplain = null,
			////SchoolFinancialInvestigationsTrustAware = null,

			// future pupil numbers
			ProjectedPupilNumbersYear1 = school.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = school.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = school.ProjectedPupilNumbersYear3,
			SchoolCapacityAssumptions = school.CapacityAssumptions,
			SchoolCapacityPublishedAdmissionsNumber = school.CapacityPublishedAdmissionsNumber,

			// land and buildings
			SchoolBuildLandOwnerExplained = school.LandAndBuildings.OwnerExplained,
			SchoolBuildLandWorksPlanned = school.LandAndBuildings.WorksPlanned,
			SchoolBuildLandWorksPlannedExplained = school.LandAndBuildings.WorksPlannedExplained,
			SchoolBuildLandWorksPlannedCompletionDate = school.LandAndBuildings.WorksPlannedDate,
			SchoolBuildLandSharedFacilities = school.LandAndBuildings.FacilitiesShared,
			SchoolBuildLandSharedFacilitiesExplained = school.LandAndBuildings.FacilitiesSharedExplained,
			SchoolBuildLandGrants = school.LandAndBuildings.Grants,
			SchoolBuildLandGrantsExplained = school.LandAndBuildings.GrantsAwardingBodies,
			SchoolBuildLandPFIScheme = school.LandAndBuildings.PartOfPfiScheme,
			SchoolBuildLandPFISchemeType = school.LandAndBuildings.PartOfPfiSchemeType,
			SchoolBuildLandPriorityBuildingProgramme = school.LandAndBuildings.PartOfPrioritySchoolsBuildingProgramme,
			SchoolBuildLandFutureProgramme = school.LandAndBuildings.PartOfBuildingSchoolsForFutureProgramme,

			// ToDo: pre-opening support grant
			////SchoolSupportGrantFundsPaidTo = null, // either "To the school" or "To the trust the school is joining"

			// ToDo: consultation details
			////SchoolHasConsultedStakeholders = null,
			////SchoolPlanToConsultStakeholders = null,

			// ToDo: declaration
			// two questions from the application form would be easy to mix up here
			// 1. I agree with all of these statements, and belive that the facts stated in this application are true (summary page)
			// 2. The information in this application is true to the best of my kowledge (actual question)
			////DeclarationBodyAgree = null,
			////DeclarationIAmTheChairOrHeadteacher = null,
			////DeclarationSignedByName = null
		};

		return serviceModel;
	}
}
