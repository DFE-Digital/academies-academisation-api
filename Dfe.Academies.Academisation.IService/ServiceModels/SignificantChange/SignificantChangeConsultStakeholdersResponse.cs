namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeConsultStakeholdersResponse
{
	public bool? TrustConsultedStakeholders { get; set; }
	public string? TrustConsultedStakeholdersNotConsultedReason { get; set; }
	public string Status { get; set; } = string.Empty;
}