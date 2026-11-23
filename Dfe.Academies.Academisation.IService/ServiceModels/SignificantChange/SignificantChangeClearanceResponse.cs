namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeClearanceResponse
{
	public string? RegionsGroupClearance { get; set; }
	public string? TrustRegulationClearance { get; set; }
	public string Status { get; set; } = string.Empty;
}
