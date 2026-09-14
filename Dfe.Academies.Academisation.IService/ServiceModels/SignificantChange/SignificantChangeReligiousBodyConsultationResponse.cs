namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeReligiousBodyConsultationResponse
{
	public bool? TrustConsultedReligiousBody { get; set; }
	public string? TrustConsultedReligiousBodyNotConsultedReason { get; set; }
	public string Status { get; set; } = string.Empty;
}