using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeLandTransactionResponse
{
	public SignificantChange_Generic_YesNoNa? LandTransactionApplication { get; set; }
	public string? LandTransactionApplicationAdditionalInfo { get; set; }
	public SignificantChange_Generic_YesNoNa? LandTransactionConsent { get; set; }
	public string? LandTransactionConsentAdditionalInfo { get; set; }
	public string? LandTransactionSupportingEvidence { get; set; }
	public string Status { get; set; } = string.Empty;
}