using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeLandTransactionResponse
{
	public YesNoNotApplicableAnswer? LandTransactionApplication { get; set; }
	public string? LandTransactionApplicationAdditionalInfo { get; set; }
	public YesNoNotApplicableAnswer? LandTransactionConsent { get; set; }
	public string? LandTransactionConsentAdditionalInfo { get; set; }
	public string? LandTransactionSupportingEvidence { get; set; }
	public string Status { get; set; } = string.Empty;
}