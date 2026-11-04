namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeFhaRisksOrIssuesRaisedResponse
{
	public bool? FhaRisksOrIssuesRaised { get; set; }
	public string? AdditionalInformation { get; set; }
	public string? SupportingEvidence { get; set; }
	public string Status { get; set; } = string.Empty;
}
