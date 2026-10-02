using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLandTransactionPublicCommand(
	SignificantChangeGenericYesNoNa? landTransactionApplication,
	string? landTransactionApplicationAdditionalInfo,
	SignificantChangeGenericYesNoNa? landTransactionConsent,
	string? landTransactionConsentAdditionalInfo,
	string? landTransactionSupportingEvidence) : IRequest<CommandResult>
{
	public SignificantChangeGenericYesNoNa? LandTransactionApplication { get; set; } = landTransactionApplication;
	public string? LandTransactionApplicationAdditionalInfo { get; set; } = landTransactionApplicationAdditionalInfo;
	public SignificantChangeGenericYesNoNa? LandTransactionConsent { get; set; } = landTransactionConsent;
	public string? LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;
	public string? LandTransactionSupportingEvidence { get; set; } = landTransactionSupportingEvidence;
}

public class SetSignificantChangeLandTransactionCommand(
	int id,
	SignificantChangeGenericYesNoNa? landTransactionApplication,
	string? landTransactionApplicationAdditionalInfo,
	SignificantChangeGenericYesNoNa? landTransactionConsent,
	string? landTransactionConsentAdditionalInfo,
	string? landTransactionSupportingEvidence)
	: SetSignificantChangeLandTransactionPublicCommand(landTransactionApplication, landTransactionApplicationAdditionalInfo, landTransactionConsent, landTransactionConsentAdditionalInfo, landTransactionSupportingEvidence)
{
	public int Id { get; set; } = id;
}