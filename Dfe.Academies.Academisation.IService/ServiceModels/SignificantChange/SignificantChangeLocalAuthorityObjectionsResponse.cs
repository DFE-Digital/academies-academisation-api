namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeLocalAuthorityObjectionsResponse
{
	public bool? LocalAuthorityRaisedObjections { get; set; }
	public string? LocalAuthorityObjectionsFurtherInformation { get; set; }
	public string? SupportingEvidenceLink { get; set; }
	public string Status { get; set; } = string.Empty;
}
