using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;

namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public class SchoolDetails
{
	protected SchoolDetails() { }

	public SchoolDetails(int Urn,
		string SchoolName,
		LandAndBuildings? LandAndBuildings,
		// additional information - split out
		// finances
		FinancialYear? PreviousFinancialYear,
		FinancialYear? CurrentFinancialYear,
		FinancialYear? NextFinancialYear,
		// contact details
		string? ContactHeadName = null,
		string? ContactHeadEmail = null,
		string? ContactChairName = null,
		string? ContactChairEmail = null,
		string? ContactRole = null, // "headteacher", "chair of governing body", "someone else"
		string? MainContactOtherName = null,
		string? MainContactOtherEmail = null,
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
		bool? FinancialInvestigationsTrustAware = null,
		// declaration
		bool? DeclarationBodyAgree = null,
		bool? DeclarationIAmTheChairOrHeadteacher = null,
		string? DeclarationSignedByName = null,
		// reasons for joining trust
		string? SchoolConversionReasonsForJoining = null)
	{
		this.Urn = Urn;
		this.SchoolName = SchoolName;
		this.LandAndBuildings = LandAndBuildings;
		this.PreviousFinancialYear = PreviousFinancialYear;
		this.CurrentFinancialYear = CurrentFinancialYear;
		this.NextFinancialYear = NextFinancialYear;
		this.ContactHeadName = ContactHeadName;
		this.ContactHeadEmail = ContactHeadEmail;
		this.ContactChairName = ContactChairName;
		this.ContactChairEmail = ContactChairEmail;
		this.ContactRole = ContactRole;
		this.MainContactOtherName = MainContactOtherName;
		this.MainContactOtherEmail = MainContactOtherEmail;
		this.MainContactOtherRole = MainContactOtherRole;
		this.ApproverContactName = ApproverContactName;
		this.ApproverContactEmail = ApproverContactEmail;
		this.ConversionTargetDateSpecified = ConversionTargetDateSpecified;
		this.ConversionTargetDate = ConversionTargetDate;
		this.ConversionTargetDateExplained = ConversionTargetDateExplained;
		this.ConversionChangeNamePlanned = ConversionChangeNamePlanned;
		this.ProposedNewSchoolName = ProposedNewSchoolName;
		this.ApplicationJoinTrustReason = ApplicationJoinTrustReason;
		this.ProjectedPupilNumbersYear1 = ProjectedPupilNumbersYear1;
		this.ProjectedPupilNumbersYear2 = ProjectedPupilNumbersYear2;
		this.ProjectedPupilNumbersYear3 = ProjectedPupilNumbersYear3;
		this.CapacityAssumptions = CapacityAssumptions;
		this.CapacityPublishedAdmissionsNumber = CapacityPublishedAdmissionsNumber;
		this.SchoolSupportGrantFundsPaidTo = SchoolSupportGrantFundsPaidTo;
		this.ConfirmPaySupportGrantToSchool = ConfirmPaySupportGrantToSchool;
		this.SchoolHasConsultedStakeholders = SchoolHasConsultedStakeholders;
		this.SchoolPlanToConsultStakeholders = SchoolPlanToConsultStakeholders;
		this.FinanceOngoingInvestigations = FinanceOngoingInvestigations;
		this.FinancialInvestigationsExplain = FinancialInvestigationsExplain;
		this.FinancialInvestigationsTrustAware = FinancialInvestigationsTrustAware;
		this.DeclarationBodyAgree = DeclarationBodyAgree;
		this.DeclarationIAmTheChairOrHeadteacher = DeclarationIAmTheChairOrHeadteacher;
		this.DeclarationSignedByName = DeclarationSignedByName;
		this.SchoolConversionReasonsForJoining = SchoolConversionReasonsForJoining;
	}

	public int Urn { get; init; }
	public string SchoolName { get; init; }
	public LandAndBuildings? LandAndBuildings { get; init; } = new();
	public FinancialYear? PreviousFinancialYear { get; init; } = new();
	public FinancialYear? CurrentFinancialYear { get; init; } = new();
	public FinancialYear? NextFinancialYear { get; init; } = new();
	public string? ContactHeadName { get; init; }
	public string? ContactHeadEmail { get; init; }
	public string? ContactChairName { get; init; }
	public string? ContactChairEmail { get; init; }
	public string? ContactRole { get; init; }
	public string? MainContactOtherName { get; init; }
	public string? MainContactOtherEmail { get; init; }
	public string? MainContactOtherRole { get; init; }
	public string? ApproverContactName { get; init; }
	public string? ApproverContactEmail { get; init; }
	public bool? ConversionTargetDateSpecified { get; init; }
	public DateTime? ConversionTargetDate { get; init; }
	public string? ConversionTargetDateExplained { get; init; }
	public bool? ConversionChangeNamePlanned { get; init; }
	public string? ProposedNewSchoolName { get; init; }
	public string? ApplicationJoinTrustReason { get; init; }
	public int? ProjectedPupilNumbersYear1 { get; init; }
	public int? ProjectedPupilNumbersYear2 { get; init; }
	public int? ProjectedPupilNumbersYear3 { get; init; }
	public string? CapacityAssumptions { get; init; }
	public int? CapacityPublishedAdmissionsNumber { get; init; }
	public PayFundsTo? SchoolSupportGrantFundsPaidTo { get; init; }
	public bool? ConfirmPaySupportGrantToSchool { get; init; }
	public bool? SchoolHasConsultedStakeholders { get; init; }
	public string? SchoolPlanToConsultStakeholders { get; init; }
	public bool? FinanceOngoingInvestigations { get; init; }
	public string? FinancialInvestigationsExplain { get; init; }
	public bool? FinancialInvestigationsTrustAware { get; init; }
	public bool? DeclarationBodyAgree { get; init; }
	public bool? DeclarationIAmTheChairOrHeadteacher { get; init; }
	public string? DeclarationSignedByName { get; init; }
	public string? SchoolConversionReasonsForJoining { get; init; }

	public void Deconstruct(out int Urn, out string SchoolName, out LandAndBuildings? LandAndBuildings,
		// additional information - split out
		// finances
		out FinancialYear? PreviousFinancialYear, out FinancialYear? CurrentFinancialYear, out FinancialYear? NextFinancialYear,
		// contact details
		out string? ContactHeadName, out string? ContactHeadEmail, out string? ContactChairName, out string? ContactChairEmail, out string? ContactRole, // "headteacher", "chair of governing body", "someone else"
		out string? MainContactOtherName, out string? MainContactOtherEmail, out string? MainContactOtherRole, out string? ApproverContactName, out string? ApproverContactEmail,
		// conversion details
		out bool? ConversionTargetDateSpecified, out DateTime? ConversionTargetDate, out string? ConversionTargetDateExplained, out bool? ConversionChangeNamePlanned, out string? ProposedNewSchoolName, out string? ApplicationJoinTrustReason,
		// future pupil numbers
		out int? ProjectedPupilNumbersYear1, out int? ProjectedPupilNumbersYear2, out int? ProjectedPupilNumbersYear3, out string? CapacityAssumptions, out int? CapacityPublishedAdmissionsNumber,
		// application pre-support grant
		out PayFundsTo? SchoolSupportGrantFundsPaidTo, out bool? ConfirmPaySupportGrantToSchool,
		// consultation details
		out bool? SchoolHasConsultedStakeholders, out string? SchoolPlanToConsultStakeholders,
		// Finances Investigations
		out bool? FinanceOngoingInvestigations, out string? FinancialInvestigationsExplain, out bool? FinancialInvestigationsTrustAware,
		// declaration
		out bool? DeclarationBodyAgree, out bool? DeclarationIAmTheChairOrHeadteacher, out string? DeclarationSignedByName,
		// reasons for joining trust
		out string? SchoolConversionReasonsForJoining)
	{
		Urn = this.Urn;
		SchoolName = this.SchoolName;
		LandAndBuildings = this.LandAndBuildings;
		PreviousFinancialYear = this.PreviousFinancialYear;
		CurrentFinancialYear = this.CurrentFinancialYear;
		NextFinancialYear = this.NextFinancialYear;
		ContactHeadName = this.ContactHeadName;
		ContactHeadEmail = this.ContactHeadEmail;
		ContactChairName = this.ContactChairName;
		ContactChairEmail = this.ContactChairEmail;
		ContactRole = this.ContactRole;
		MainContactOtherName = this.MainContactOtherName;
		MainContactOtherEmail = this.MainContactOtherEmail;
		MainContactOtherRole = this.MainContactOtherRole;
		ApproverContactName = this.ApproverContactName;
		ApproverContactEmail = this.ApproverContactEmail;
		ConversionTargetDateSpecified = this.ConversionTargetDateSpecified;
		ConversionTargetDate = this.ConversionTargetDate;
		ConversionTargetDateExplained = this.ConversionTargetDateExplained;
		ConversionChangeNamePlanned = this.ConversionChangeNamePlanned;
		ProposedNewSchoolName = this.ProposedNewSchoolName;
		ApplicationJoinTrustReason = this.ApplicationJoinTrustReason;
		ProjectedPupilNumbersYear1 = this.ProjectedPupilNumbersYear1;
		ProjectedPupilNumbersYear2 = this.ProjectedPupilNumbersYear2;
		ProjectedPupilNumbersYear3 = this.ProjectedPupilNumbersYear3;
		CapacityAssumptions = this.CapacityAssumptions;
		CapacityPublishedAdmissionsNumber = this.CapacityPublishedAdmissionsNumber;
		SchoolSupportGrantFundsPaidTo = this.SchoolSupportGrantFundsPaidTo;
		ConfirmPaySupportGrantToSchool = this.ConfirmPaySupportGrantToSchool;
		SchoolHasConsultedStakeholders = this.SchoolHasConsultedStakeholders;
		SchoolPlanToConsultStakeholders = this.SchoolPlanToConsultStakeholders;
		FinanceOngoingInvestigations = this.FinanceOngoingInvestigations;
		FinancialInvestigationsExplain = this.FinancialInvestigationsExplain;
		FinancialInvestigationsTrustAware = this.FinancialInvestigationsTrustAware;
		DeclarationBodyAgree = this.DeclarationBodyAgree;
		DeclarationIAmTheChairOrHeadteacher = this.DeclarationIAmTheChairOrHeadteacher;
		DeclarationSignedByName = this.DeclarationSignedByName;
		SchoolConversionReasonsForJoining = this.SchoolConversionReasonsForJoining;
	}
}
