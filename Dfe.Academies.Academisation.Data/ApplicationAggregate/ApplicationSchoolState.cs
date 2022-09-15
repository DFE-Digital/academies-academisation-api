using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate;

[Table(name: "ApplicationSchool")]
public class ApplicationSchoolState : BaseEntity
{
	public int Urn { get; set; }
	public string SchoolName { get; set; } = null!;

	// contact details
	public string? ContactHeadName { get; set; }
	public string? ContactHeadEmail { get; set; }
	public string? ContactHeadTel { get; set; }
	public string? ContactChairName { get; set; }
	public string? ContactChairEmail { get; set; }
	public string? ContactChairTel { get; set; }
	public string? ContactRole { get; set; } // "headteacher", "chair of governing body", "someone else"
	public string? MainContactOtherName { get; set; }
	public string? MainContactOtherEmail { get; set; }
	public string? MainContactOtherTelephone { get; set; }
	public string? MainContactOtherRole { get; set; }
	public string? ApproverContactName { get; set; }
	public string? ApproverContactEmail { get; set; }

	// conversion details
	public bool? ConversionTargetDateSpecified { get; set; }
	public DateTime? ConversionTargetDate { get; set; }
	public string? ConversionTargetDateExplained { get; set; }
	public bool? ConversionChangeNamePlanned { get; set; }
	public string? ProposedNewSchoolName { get; set; }
	public string? JoinTrustReason { get; set; }

	// future pupil numbers
	public int? ProjectedPupilNumbersYear1 { get; set; }
	public int? ProjectedPupilNumbersYear2 { get; set; }
	public int? ProjectedPupilNumbersYear3 { get; set; }
	public string? CapacityAssumptions { get; set; }
	public int? CapacityPublishedAdmissionsNumber { get; set; }

	// land and buildings
	public string? OwnerExplained { get; set; }
	public bool? WorksPlanned { get; set; }
	public DateTime? WorksPlannedDate { get; set; }
	public string? WorksPlannedExplained { get; set; }
	public bool? FacilitiesShared { get; set; }
	public string? FacilitiesSharedExplained { get; set; }
	public bool? Grants { get; set; }
	public string? GrantsAwardingBodies { get; set; }
	public bool? PartOfPfiScheme { get; set; }
	public string? PartOfPfiSchemeType { get; set; }
	public bool? PartOfPrioritySchoolsBuildingProgramme { get; set; }
	public bool? PartOfBuildingSchoolsForFutureProgramme { get; set; }
	// application pre-support grant
	public PayFundsTo? SupportGrantFundsPaidTo { get; set; }
	public bool? ConfirmPaySupportGrantToSchool { get; set; }

	// additional information - for data storage - store them flat !
	// Performance
	public bool? InspectedButReportNotPublished { get; set; }
	public string? InspectedButReportNotPublishedExplain { get; set; }
	public bool? OngoingSafeguardingInvestigations { get; set; }
	public string? OngoingSafeguardingDetails { get; set; }
	// LocalAuthority
	public bool? PartOfLaReorganizationPlan { get; set; }
	public string? LaReorganizationDetails { get; set; }
	public bool? PartOfLaClosurePlan { get; set; }
	public string? LaClosurePlanDetails { get; set; }
	// PartnershipsAndAffliations
	public bool? IsPartOfFederation { get; set; }
	public bool? IsSupportedByFoundation { get; set; }
	public string? SupportedFoundationName { get; set; }
	public string? SupportedFoundationEvidenceDocumentLink { get; set; }
	public string? FeederSchools { get; set; }
	// religion
	public bool? FaithSchool { get; set; }
	public string? FaithSchoolDioceseName { get; set; }
	public string? DiocesePermissionEvidenceDocumentLink { get; set; }
	public bool? HasSACREException { get; set; }
	public DateTime? SACREExemptionEndDate { get; set; }
	// other additional information
	public string? SchoolContributionToTrust { get; set; }
	public string? GoverningBodyConsentEvidenceDocumentLink { get; set; }
	public bool? AdditionalInformationAdded { get; set; }
	public string? AdditionalInformation { get; set; }
	public EqualityImpact? EqualitiesImpactAssessmentCompleted { get; set; }
	public string? EqualitiesImpactAssessmentDetails { get; set; }
	// previous financial year
	public DateTime? PreviousFinancialYearEndDate { get; set; }
	public decimal? PreviousFinancialYearRevenue { get; set; }
	public RevenueType? PreviousFinancialYearRevenueStatus { get; set; }
	public string? PreviousFinancialYearRevenueStatusExplained { get; set; }
	public string? PreviousFinancialYearRevenueStatusFileLink { get; set; }
	public decimal? PreviousFinancialYearCapitalCarryForward { get; set; }
	public RevenueType? PreviousFinancialYearCapitalCarryForwardStatus { get; set; }
	public string? PreviousFinancialYearCapitalCarryForwardExplained { get; set; }
	public string? PreviousFinancialYearCapitalCarryForwardFileLink { get; set; }
	// current fin yr
	public DateTime? CurrentFinancialYearEndDate { get; set; }
	public decimal? CurrentFinancialYearRevenue { get; set; }
	public RevenueType? CurrentFinancialYearRevenueStatus { get; set; }
	public string? CurrentFinancialYearRevenueStatusExplained { get; set; }
	public string? CurrentFinancialYearRevenueStatusFileLink { get; set; }
	public decimal? CurrentFinancialYearCapitalCarryForward { get; set; }
	public RevenueType? CurrentFinancialYearCapitalCarryForwardStatus { get; set; }
	public string? CurrentFinancialYearCapitalCarryForwardExplained { get; set; }
	public string? CurrentFinancialYearCapitalCarryForwardFileLink { get; set; }
	// next financial year
	public DateTime? NextFinancialYearEndDate { get; set; }
	public decimal? NextFinancialYearRevenue { get; set; }
	public RevenueType? NextFinancialYearRevenueStatus { get; set; }
	public string? NextFinancialYearRevenueStatusExplained { get; set; }
	public string? NextFinancialYearRevenueStatusFileLink { get; set; }
	public decimal? NextFinancialYearCapitalCarryForward { get; set; }
	public RevenueType? NextFinancialYearCapitalCarryForwardStatus { get; set; }
	public string? NextFinancialYearCapitalCarryForwardExplained { get; set; }
	public string? NextFinancialYearCapitalCarryForwardFileLink { get; set; }

	// leases & loans
	[ForeignKey("ApplicationSchoolId")]
	public HashSet<LoanState> Loans { get; set; } = new();

	public static ApplicationSchoolState MapFromDomain(ISchool applyingSchool)
	{
		return new()
		{
			Id = applyingSchool.Id,
			Urn = applyingSchool.Details.Urn,
			SchoolName = applyingSchool.Details.SchoolName,
			ContactRole = applyingSchool.Details.ContactRole,
			ApproverContactEmail = applyingSchool.Details.ApproverContactEmail,
			ApproverContactName = applyingSchool.Details.ApproverContactName,
			ContactChairEmail = applyingSchool.Details.ContactChairEmail,
			ContactChairName = applyingSchool.Details.ContactChairName,
			ContactChairTel = applyingSchool.Details.ContactChairTel,
			ContactHeadEmail = applyingSchool.Details.ContactHeadEmail,
			ContactHeadName = applyingSchool.Details.ContactHeadName,
			ContactHeadTel = applyingSchool.Details.ContactHeadTel,
			MainContactOtherEmail = applyingSchool.Details.MainContactOtherEmail,
			MainContactOtherName = applyingSchool.Details.MainContactOtherName,
			MainContactOtherRole = applyingSchool.Details.MainContactOtherRole,
			MainContactOtherTelephone = applyingSchool.Details.MainContactOtherTelephone,
			JoinTrustReason = applyingSchool.Details.ApplicationJoinTrustReason,
			ConversionTargetDateSpecified = applyingSchool.Details.ConversionTargetDateSpecified,
			ConversionTargetDate = applyingSchool.Details.ConversionTargetDate,
			ConversionTargetDateExplained = applyingSchool.Details.ConversionTargetDateExplained,
			ConversionChangeNamePlanned = applyingSchool.Details.ConversionChangeNamePlanned,
			ProposedNewSchoolName = applyingSchool.Details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = applyingSchool.Details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = applyingSchool.Details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = applyingSchool.Details.ProjectedPupilNumbersYear3,
			CapacityAssumptions = applyingSchool.Details.CapacityAssumptions,
			CapacityPublishedAdmissionsNumber = applyingSchool.Details.CapacityPublishedAdmissionsNumber,
			OwnerExplained = applyingSchool.Details.LandAndBuildings.OwnerExplained,
			WorksPlanned = applyingSchool.Details.LandAndBuildings.WorksPlanned,
			WorksPlannedDate = applyingSchool.Details.LandAndBuildings.WorksPlannedDate,
			WorksPlannedExplained = applyingSchool.Details.LandAndBuildings.WorksPlannedExplained,
			FacilitiesShared = applyingSchool.Details.LandAndBuildings.FacilitiesShared,
			FacilitiesSharedExplained = applyingSchool.Details.LandAndBuildings.FacilitiesSharedExplained,
			Grants = applyingSchool.Details.LandAndBuildings.Grants,
			GrantsAwardingBodies = applyingSchool.Details.LandAndBuildings.GrantsAwardingBodies,
			PartOfPfiScheme = applyingSchool.Details.LandAndBuildings.PartOfPfiScheme,
			PartOfPfiSchemeType = applyingSchool.Details.LandAndBuildings.PartOfPfiSchemeType,
			PartOfPrioritySchoolsBuildingProgramme = applyingSchool.Details.LandAndBuildings.PartOfPrioritySchoolsBuildingProgramme,
			PartOfBuildingSchoolsForFutureProgramme = applyingSchool.Details.LandAndBuildings.PartOfBuildingSchoolsForFutureProgramme,
			SupportGrantFundsPaidTo = applyingSchool.Details.SchoolSupportGrantFundsPaidTo,
			ConfirmPaySupportGrantToSchool = applyingSchool.Details.ConfirmPaySupportGrantToSchool,
			// additional information
			// Performance
			InspectedButReportNotPublished = applyingSchool.Details.Performance.InspectedButReportNotPublished,
			InspectedButReportNotPublishedExplain = applyingSchool.Details.Performance.InspectedButReportNotPublishedExplain,
			OngoingSafeguardingInvestigations = applyingSchool.Details.Performance.OngoingSafeguardingInvestigations,
			OngoingSafeguardingDetails = applyingSchool.Details.Performance.OngoingSafeguardingDetails,

			// LocalAuthority
			PartOfLaReorganizationPlan = applyingSchool.Details.LocalAuthority.PartOfLaReorganizationPlan,
			LaReorganizationDetails = applyingSchool.Details.LocalAuthority.LaReorganizationDetails,
			PartOfLaClosurePlan = applyingSchool.Details.LocalAuthority.PartOfLaClosurePlan,
			LaClosurePlanDetails = applyingSchool.Details.LocalAuthority.LaClosurePlanDetails,
			// PartnershipsAndAffliations
			IsPartOfFederation = applyingSchool.Details.PartnershipsAndAffliations.IsPartOfFederation,
			IsSupportedByFoundation = applyingSchool.Details.PartnershipsAndAffliations.IsSupportedByFoundation,
			SupportedFoundationName = applyingSchool.Details.PartnershipsAndAffliations.SupportedFoundationName,
			SupportedFoundationEvidenceDocumentLink = applyingSchool.Details.PartnershipsAndAffliations.SupportedFoundationEvidenceDocumentLink,
			FeederSchools = applyingSchool.Details.PartnershipsAndAffliations.FeederSchools,
			// religion
			FaithSchool = applyingSchool.Details.ReligiousEducation.FaithSchool,
			FaithSchoolDioceseName = applyingSchool.Details.ReligiousEducation.FaithSchoolDioceseName,
			DiocesePermissionEvidenceDocumentLink = applyingSchool.Details.ReligiousEducation.DiocesePermissionEvidenceDocumentLink,
			HasSACREException = applyingSchool.Details.ReligiousEducation.HasSACREException,
			SACREExemptionEndDate = applyingSchool.Details.ReligiousEducation.SACREExemptionEndDate,
			// other additional information
			SchoolContributionToTrust = applyingSchool.Details.SchoolContributionToTrust,
			GoverningBodyConsentEvidenceDocumentLink = applyingSchool.Details.GoverningBodyConsentEvidenceDocumentLink,
			AdditionalInformationAdded = applyingSchool.Details.AdditionalInformationAdded,
			AdditionalInformation = applyingSchool.Details.AdditionalInformation,
			EqualitiesImpactAssessmentCompleted = applyingSchool.Details.EqualitiesImpactAssessmentCompleted, 
			EqualitiesImpactAssessmentDetails = applyingSchool.Details.EqualitiesImpactAssessmentDetails,
			// previous financial yr
			PreviousFinancialYearEndDate = applyingSchool.Details.PreviousFinancialYear.FinancialYearEndDate,
			PreviousFinancialYearRevenue = applyingSchool.Details.PreviousFinancialYear.Revenue,
			PreviousFinancialYearRevenueStatus = applyingSchool.Details.PreviousFinancialYear.RevenueStatus,
			PreviousFinancialYearRevenueStatusExplained = applyingSchool.Details.PreviousFinancialYear.RevenueStatusExplained,
			PreviousFinancialYearRevenueStatusFileLink = applyingSchool.Details.PreviousFinancialYear.RevenueStatusFileLink,
			PreviousFinancialYearCapitalCarryForward = applyingSchool.Details.PreviousFinancialYear.CapitalCarryForward,
			PreviousFinancialYearCapitalCarryForwardStatus = applyingSchool.Details.PreviousFinancialYear.CapitalCarryForwardStatus,
			PreviousFinancialYearCapitalCarryForwardExplained = applyingSchool.Details.PreviousFinancialYear.CapitalCarryForwardExplained,
			PreviousFinancialYearCapitalCarryForwardFileLink = applyingSchool.Details.PreviousFinancialYear.CapitalCarryForwardFileLink,
			// current financial yr
			CurrentFinancialYearEndDate = applyingSchool.Details.CurrentFinancialYear.FinancialYearEndDate,
			CurrentFinancialYearRevenue = applyingSchool.Details.CurrentFinancialYear.Revenue,
			CurrentFinancialYearRevenueStatus = applyingSchool.Details.CurrentFinancialYear.RevenueStatus,
			CurrentFinancialYearRevenueStatusExplained = applyingSchool.Details.CurrentFinancialYear.RevenueStatusExplained,
			CurrentFinancialYearRevenueStatusFileLink = applyingSchool.Details.CurrentFinancialYear.RevenueStatusFileLink,
			CurrentFinancialYearCapitalCarryForward = applyingSchool.Details.CurrentFinancialYear.CapitalCarryForward,
			CurrentFinancialYearCapitalCarryForwardStatus = applyingSchool.Details.CurrentFinancialYear.CapitalCarryForwardStatus,
			CurrentFinancialYearCapitalCarryForwardExplained = applyingSchool.Details.CurrentFinancialYear.CapitalCarryForwardExplained,
			CurrentFinancialYearCapitalCarryForwardFileLink = applyingSchool.Details.CurrentFinancialYear.CapitalCarryForwardFileLink,
			// next financial year
			NextFinancialYearEndDate = applyingSchool.Details.NextFinancialYear.FinancialYearEndDate,
			NextFinancialYearRevenue = applyingSchool.Details.NextFinancialYear.Revenue,
			NextFinancialYearRevenueStatus = applyingSchool.Details.NextFinancialYear.RevenueStatus,
			NextFinancialYearRevenueStatusExplained = applyingSchool.Details.NextFinancialYear.RevenueStatusExplained,
			NextFinancialYearRevenueStatusFileLink = applyingSchool.Details.NextFinancialYear.RevenueStatusFileLink,
			NextFinancialYearCapitalCarryForward = applyingSchool.Details.NextFinancialYear.CapitalCarryForward,
			NextFinancialYearCapitalCarryForwardStatus = applyingSchool.Details.NextFinancialYear.CapitalCarryForwardStatus,
			NextFinancialYearCapitalCarryForwardExplained = applyingSchool.Details.NextFinancialYear.CapitalCarryForwardExplained,
			NextFinancialYearCapitalCarryForwardFileLink = applyingSchool.Details.NextFinancialYear.CapitalCarryForwardFileLink,
			// TODO MR:- loans
			Loans = new HashSet<LoanState>(applyingSchool.Loans
				?.Select(e => new LoanState
				{
					Id = e.Id,
					Amount = e.Details.Amount,
					Purpose = e.Details.Purpose,
					Provider = e.Details.Provider,
					InterestRate = e.Details.InterestRate,
					Schedule = e.Details.Schedule
				})
				.ToList() ?? new List<LoanState>())
		};
	}

	public School MapToDomain()
	{
		var schoolDetails = new SchoolDetails(
			Urn,
			SchoolName,
			new LandAndBuildings
			{
				OwnerExplained = OwnerExplained,
				WorksPlanned = WorksPlanned,
				WorksPlannedDate = WorksPlannedDate,
				WorksPlannedExplained = WorksPlannedExplained,
				FacilitiesShared = FacilitiesShared,
				FacilitiesSharedExplained = FacilitiesSharedExplained,
				Grants = Grants,
				GrantsAwardingBodies = GrantsAwardingBodies,
				PartOfPfiScheme = PartOfPfiScheme,
				PartOfPfiSchemeType = PartOfPfiSchemeType,
				PartOfPrioritySchoolsBuildingProgramme = PartOfPrioritySchoolsBuildingProgramme,
				PartOfBuildingSchoolsForFutureProgramme = PartOfBuildingSchoolsForFutureProgramme
			},
			new Performance
			{
				InspectedButReportNotPublished = InspectedButReportNotPublished,
				InspectedButReportNotPublishedExplain = InspectedButReportNotPublishedExplain,
				OngoingSafeguardingDetails = OngoingSafeguardingDetails,
				OngoingSafeguardingInvestigations = OngoingSafeguardingInvestigations
			},
			new LocalAuthority
			{
				PartOfLaReorganizationPlan = PartOfLaReorganizationPlan,
				LaClosurePlanDetails = LaClosurePlanDetails,
				PartOfLaClosurePlan = PartOfLaClosurePlan,
				LaReorganizationDetails = LaReorganizationDetails
			},
			new PartnershipsAndAffliations
			{
				IsPartOfFederation = IsPartOfFederation,
				IsSupportedByFoundation = IsSupportedByFoundation,
				SupportedFoundationName = SupportedFoundationName,
				SupportedFoundationEvidenceDocumentLink = SupportedFoundationEvidenceDocumentLink,
				FeederSchools = FeederSchools
			},
			new ReligiousEducation
			{
				FaithSchool = FaithSchool,
				FaithSchoolDioceseName = FaithSchoolDioceseName,
				DiocesePermissionEvidenceDocumentLink = DiocesePermissionEvidenceDocumentLink,
				HasSACREException = HasSACREException,
				SACREExemptionEndDate = SACREExemptionEndDate
			},
			// previous financial yr
			new FinancialYear
			{
				FinancialYearEndDate = PreviousFinancialYearEndDate,
				Revenue = PreviousFinancialYearRevenue,
				RevenueStatus = PreviousFinancialYearRevenueStatus,
				RevenueStatusExplained = PreviousFinancialYearRevenueStatusExplained,
				RevenueStatusFileLink = PreviousFinancialYearRevenueStatusFileLink,
				CapitalCarryForward = PreviousFinancialYearCapitalCarryForward,
				CapitalCarryForwardStatus = PreviousFinancialYearCapitalCarryForwardStatus,
				CapitalCarryForwardExplained = PreviousFinancialYearCapitalCarryForwardExplained,
				CapitalCarryForwardFileLink = PreviousFinancialYearCapitalCarryForwardFileLink
			},
			// current financial year
			new FinancialYear
			{
				FinancialYearEndDate = CurrentFinancialYearEndDate,
				Revenue = CurrentFinancialYearRevenue,
				RevenueStatus = CurrentFinancialYearRevenueStatus,
				RevenueStatusExplained = CurrentFinancialYearRevenueStatusExplained,
				RevenueStatusFileLink = CurrentFinancialYearRevenueStatusFileLink,
				CapitalCarryForward = CurrentFinancialYearCapitalCarryForward,
				CapitalCarryForwardStatus = CurrentFinancialYearCapitalCarryForwardStatus,
				CapitalCarryForwardExplained = CurrentFinancialYearCapitalCarryForwardExplained,
				CapitalCarryForwardFileLink = CurrentFinancialYearCapitalCarryForwardFileLink
			},
			// next financial year 
			new FinancialYear
			{
				FinancialYearEndDate = NextFinancialYearEndDate,
				Revenue = NextFinancialYearRevenue,
				RevenueStatus = NextFinancialYearRevenueStatus,
				RevenueStatusExplained = NextFinancialYearRevenueStatusExplained,
				RevenueStatusFileLink = NextFinancialYearRevenueStatusFileLink,
				CapitalCarryForward = NextFinancialYearCapitalCarryForward,
				CapitalCarryForwardStatus = NextFinancialYearCapitalCarryForwardStatus,
				CapitalCarryForwardExplained = NextFinancialYearCapitalCarryForwardExplained,
				CapitalCarryForwardFileLink = NextFinancialYearCapitalCarryForwardFileLink
			})
			{
			SchoolContributionToTrust = SchoolContributionToTrust,
			GoverningBodyConsentEvidenceDocumentLink = GoverningBodyConsentEvidenceDocumentLink,
			AdditionalInformationAdded = AdditionalInformationAdded,
			AdditionalInformation = AdditionalInformation,
			EqualitiesImpactAssessmentCompleted = EqualitiesImpactAssessmentCompleted,
			EqualitiesImpactAssessmentDetails = EqualitiesImpactAssessmentDetails,
			ContactRole = ContactRole,
			ApproverContactEmail = ApproverContactEmail,
			ApproverContactName = ApproverContactName,
			ContactChairEmail = ContactChairEmail,
			ContactChairName = ContactChairName,
			ContactChairTel = ContactChairTel,
			ContactHeadEmail = ContactHeadEmail,
			ContactHeadName = ContactHeadName,
			ContactHeadTel = ContactHeadTel,
			MainContactOtherEmail = MainContactOtherEmail,
			MainContactOtherName = MainContactOtherName,
			MainContactOtherRole = MainContactOtherRole,
			MainContactOtherTelephone = MainContactOtherTelephone,
			ApplicationJoinTrustReason = JoinTrustReason,
			ConversionTargetDateSpecified = ConversionTargetDateSpecified,
			ConversionTargetDate = ConversionTargetDate,
			ConversionTargetDateExplained = ConversionTargetDateExplained,
			ProposedNewSchoolName = ProposedNewSchoolName,
			ConversionChangeNamePlanned = ConversionChangeNamePlanned,
			ProjectedPupilNumbersYear1 = ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = ProjectedPupilNumbersYear3,
			CapacityAssumptions = CapacityAssumptions,
			CapacityPublishedAdmissionsNumber = CapacityPublishedAdmissionsNumber,
			SchoolSupportGrantFundsPaidTo = SupportGrantFundsPaidTo,
			ConfirmPaySupportGrantToSchool = ConfirmPaySupportGrantToSchool
		};

		return new School(Id, schoolDetails, Loans.Select(n => n.MapToDomain()));
	}
}
