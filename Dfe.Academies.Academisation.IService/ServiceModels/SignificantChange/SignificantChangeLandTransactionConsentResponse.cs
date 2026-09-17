using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeLandTransactionResponse
{
	public SignificantChangeLandTransactionConsent? LandTransactionConsentSecured { get; set; }
	public string? LandTransactionConsentAdditionalInfo { get; set; }
	public string Status { get; set; } = string.Empty;
}