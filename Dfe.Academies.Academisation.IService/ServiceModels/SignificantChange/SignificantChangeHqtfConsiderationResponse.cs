using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeHqtfConsiderationResponse
{
	public HqtfConsiderationAnswer? HqtfConsiderationAnswer { get; set; }
	public string? AdditionalInformation { get; set; }
	public string? SupportingEvidence { get; set; }
	public string Status { get; set; } = string.Empty;
}
