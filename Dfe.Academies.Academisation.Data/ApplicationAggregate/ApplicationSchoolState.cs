using System.ComponentModel.DataAnnotations.Schema;
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
	// TODO MR:- all below:-
	//string? SchoolAdSchoolContributionToTrust = null,
	//bool? SchoolOngoingSafeguardingInvestigations = null,
	//string? SchoolOngoingSafeguardingDetails = null,
	//bool? SchoolPartOfLaReorganizationPlan = null,
	//string? SchoolLaReorganizationDetails = null,
	//bool? SchoolPartOfLaClosurePlan = null,
	//string? SchoolLaClosurePlanDetails = null,
	//bool? SchoolFaithSchool = null,
	//string? SchoolFaithSchoolDioceseName = null,
	//string? DiocesePermissionEvidenceDocumentLink = null,
	//bool? SchoolIsPartOfFederation = null,
	//bool? SchoolIsSupportedByFoundation = null,
	//string? SchoolSupportedFoundationBodyName = null,
	//string? FoundationEvidenceDocumentLink = null,
	//bool? SchoolHasSACREException = null,
	//	DateTime? SchoolSACREExemptionEndDate = null,
	//string? SchoolAdFeederSchools = null,
	//string? GoverningBodyConsentEvidenceDocumentLink = null,
	//bool? SchoolAdEqualitiesImpactAssessmentCompleted = null,
	//string? SchoolAdEqualitiesImpactAssessmentDetails = null, // two possible very long proforma string? answers here - maybe this should be a bool
	//bool? SchoolAdInspectedButReportNotPublished = null,
	//string? SchoolAdInspectedButReportNotPublishedExplain = null,
	//bool? SchoolAdditionalInformationAdded = null,
	//string? SchoolAdditionalInformation = null,

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
			// TODO MR:-
		};
	}

	public SchoolDetails MapToDomain()
	{
		return new SchoolDetails(
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
			})
		{
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
			ConfirmPaySupportGrantToSchool = ConfirmPaySupportGrantToSchool,
			// additional information
			// TODO MR:-
		};
	}
}
