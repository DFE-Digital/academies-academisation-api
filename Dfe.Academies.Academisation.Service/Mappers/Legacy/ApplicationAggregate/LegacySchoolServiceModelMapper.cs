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
			// ToDo: Loans - set from School.Loans
			new List<LegacyLoanServiceModel>(),
			// ToDo: Leases - set from School.Leases
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
			SchoolConversionTargetDateSpecified = school.ConversionTargetDateSpecified,
			SchoolConversionTargetDate = school.ConversionTargetDate,
			SchoolConversionTargetDateExplained = school.ConversionTargetDateExplained,

			// reasons for joining
			SchoolConversionReasonsForJoining = school.ApplicationJoinTrustReason,

			// name changes
			SchoolConversionChangeNamePlanned = school.ConversionChangeNamePlanned,
			SchoolConversionProposedNewSchoolName = school.ProposedNewSchoolName,

			// additional information
			// Performance
			SchoolAdInspectedButReportNotPublished = school.Performance.InspectedButReportNotPublished,
			SchoolAdInspectedButReportNotPublishedExplain = school.Performance.InspectedButReportNotPublishedExplain,
			SchoolOngoingSafeguardingInvestigations = school.Performance.OngoingSafeguardingInvestigations,
			SchoolOngoingSafeguardingDetails = school.Performance.OngoingSafeguardingDetails,
			// LocalAuthority
			SchoolPartOfLaReorganizationPlan = school.LocalAuthority.PartOfLaReorganizationPlan,
			SchoolLaReorganizationDetails = school.LocalAuthority.LaReorganizationDetails,
			SchoolPartOfLaClosurePlan = school.LocalAuthority.PartOfLaClosurePlan,
			SchoolLaClosurePlanDetails = school.LocalAuthority.LaClosurePlanDetails,
			//PartnershipsAndAffliations
			SchoolIsPartOfFederation = school.PartnershipsAndAffliations.IsPartOfFederation,
			SchoolIsSupportedByFoundation = school.PartnershipsAndAffliations.IsSupportedByFoundation,
			SchoolSupportedFoundationBodyName = school.PartnershipsAndAffliations.SupportedFoundationName,
			FoundationEvidenceDocumentLink = school.PartnershipsAndAffliations.SupportedFoundationEvidenceDocumentLink,
			SchoolAdFeederSchools = school.PartnershipsAndAffliations.FeederSchools,
			// ReligiousEducation
			SchoolFaithSchool = school.ReligiousEducation.FaithSchool,
			SchoolFaithSchoolDioceseName = school.ReligiousEducation.FaithSchoolDioceseName,
			DiocesePermissionEvidenceDocumentLink = school.ReligiousEducation.DiocesePermissionEvidenceDocumentLink,
			SchoolHasSACREException = school.ReligiousEducation.HasSACREException,
			SchoolSACREExemptionEndDate = school.ReligiousEducation.SACREExemptionEndDate,
			// other additional information
			SchoolAdSchoolContributionToTrust = school.SchoolContributionToTrust,
			GoverningBodyConsentEvidenceDocumentLink = school.GoverningBodyConsentEvidenceDocumentLink,
			SchoolAdditionalInformationAdded = school.AdditionalInformationAdded,
			SchoolAdditionalInformation = school.AdditionalInformation,
			SchoolAdEqualitiesImpactAssessmentCompleted = !string.IsNullOrWhiteSpace(school.EqualitiesImpactAssessmentDetails),
			SchoolAdEqualitiesImpactAssessmentDetails = school.EqualitiesImpactAssessmentDetails,
			// Finances
			PreviousFinancialYear = new LegacyFinancialYearServiceModel
			{
				FYEndDate = school.PreviousFinancialYear.FinancialYearEndDate,
				RevenueCarryForward = school.PreviousFinancialYear.Revenue,
				RevenueStatusExplained = school.PreviousFinancialYear.RevenueStatusExplained,
				CapitalCarryForward = school.PreviousFinancialYear.CapitalCarryForward,
				CapitalIsDeficit = school.PreviousFinancialYear.CapitalCarryForwardStatus == RevenueType.Deficit,
				CapitalStatusExplained = school.PreviousFinancialYear.CapitalCarryForwardExplained
			}, 
			CurrentFinancialYear = new LegacyFinancialYearServiceModel
			{
				FYEndDate = school.CurrentFinancialYear.FinancialYearEndDate,
				RevenueCarryForward = school.CurrentFinancialYear.Revenue,
				RevenueStatusExplained = school.CurrentFinancialYear.RevenueStatusExplained,
				CapitalCarryForward = school.CurrentFinancialYear.CapitalCarryForward,
				CapitalIsDeficit = school.CurrentFinancialYear.CapitalCarryForwardStatus == RevenueType.Deficit,
				CapitalStatusExplained = school.CurrentFinancialYear.CapitalCarryForwardExplained
			},
			NextFinancialYear = new LegacyFinancialYearServiceModel
			{
				FYEndDate = school.NextFinancialYear.FinancialYearEndDate,
				RevenueCarryForward = school.NextFinancialYear.Revenue,
				RevenueStatusExplained = school.NextFinancialYear.RevenueStatusExplained,
				CapitalCarryForward = school.NextFinancialYear.CapitalCarryForward,
				CapitalIsDeficit = school.NextFinancialYear.CapitalCarryForwardStatus == RevenueType.Deficit,
				CapitalStatusExplained = school.NextFinancialYear.CapitalCarryForwardExplained
			},
			// ToDo: Finances - MK2
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

			// pre-opening support grant
			SchoolSupportGrantFundsPaidTo = MapType(school), // either "To the school" or "To the trust the school is joining"

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

	private static string MapType(SchoolDetails school)
	{
		return school.SchoolSupportGrantFundsPaidTo switch
		{
			PayFundsTo.School => "To the school",
			PayFundsTo.Trust => "To the trust the school is joining",
			_ => throw new NotImplementedException("Unrecognised PayFundsTo, has a new PayFundsTo been added recently?"),
		};
	}
}
