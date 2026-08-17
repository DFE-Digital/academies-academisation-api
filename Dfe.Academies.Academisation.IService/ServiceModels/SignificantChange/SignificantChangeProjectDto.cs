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
	public string Status { get; set; } = string.Empty;
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public string StakeholderConsultationTaskStatus { get; set; } = string.Empty;

	public bool? ConsultationIncludeAdmissionVariation { get; set; }
	public string? ConsultationNoAdmissionVariationReason { get; set; }
	public string AdmissionVariationConsultationTaskStatus { get; set; } = string.Empty;
}
