using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeProjectDto
{
	public int Id { get; set; }
	public int Urn { get; set; }
	public string SchoolName { get; set; } = string.Empty;
	public byte Tier { get; set; }
	public string TrustName { get; set; } = string.Empty;
	public string TrustUkprn { get; set; } = string.Empty;
	public Guid? AssignedUserId { get; set; }
	public string? AssignedUserFullName { get; set; }
	public string? AssignedUserEmailAddress { get; set; }
	public string TypeOfSignificantChange { get; set; } = string.Empty;
	public string? ApplicationId { get; set; }
	public string? ApplicationReference { get; set; }
	public string? LocalAuthorityName { get; set; }
	public string? CompaniesHouseNumber { get; set; }
	public string Status { get; set; } = string.Empty;
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public string StakeholderConsultationTaskStatus { get; set; } = string.Empty;
	public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
	public string? ConsultationDurationNotMetReason { get; set; }
	public string ConsultationDurationTaskStatus { get; set; } = string.Empty;


	public bool? ConsultationIncludeAdmissionVariation { get; set; }
	public string? ConsultationNoAdmissionVariationReason { get; set; }
	public string AdmissionVariationConsultationTaskStatus { get; set; } = string.Empty;
	public bool? LocalAuthorityRaisedObjections { get; set; }
	public string? LocalAuthorityObjectionsFurtherInformation { get; set; }
	public string? SupportingEvidenceLink { get; set; }
	public string LocalAuthorityObjectionsTaskStatus { get; set; } = string.Empty;
	public string EqualitiesTaskStatus { get; set; } = string.Empty;
	public bool? EqualitiesImpactAssessmentCompleted { get; set; }
	public string? EqualitiesImpactIdentified { get; set; }
	public string? EqualitiesImpactIdentifiedMitigation { get; set; }
	public bool? TrustConsultedReligiousBody { get; set; }
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; }
	public string ReligiousBodyConsultationTaskStatus { get; set; } = string.Empty;
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }
	public string ConfirmProjectDatesTaskStatus { get; set; } = string.Empty;
	public string? StakeholderObjections { get; set; }
	public string? StakeholderObjectionsComment { get; set; }
	public string StakeholderObjectionsTaskStatus { get; set; } = string.Empty;
}
