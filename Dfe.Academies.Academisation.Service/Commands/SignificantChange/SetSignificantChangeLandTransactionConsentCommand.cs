using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLandTransactionPublicCommand(
	SignificantChange_Generic_YesNoNa? landTransactionApplication,
	string? landTransactionApplicationAdditionalInfo,
	SignificantChange_Generic_YesNoNa? landTransactionConsent,
	string? landTransactionConsentAdditionalInfo,
	string? landTransactionSupportingEvidence) : IRequest<CommandResult>
{
	public SignificantChange_Generic_YesNoNa? LandTransactionApplication { get; set; } = landTransactionApplication;
	public string? LandTransactionApplicationAdditionalInfo { get; set; } = landTransactionApplicationAdditionalInfo;
	public SignificantChange_Generic_YesNoNa? LandTransactionConsent { get; set; } = landTransactionConsent;
	public string? LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;
	public string? LandTransactionSupportingEvidence { get; set; } = landTransactionSupportingEvidence;
}

public class SetSignificantChangeLandTransactionCommand(
	int id,
	SignificantChange_Generic_YesNoNa? landTransactionApplication,
	string? landTransactionApplicationAdditionalInfo,
	SignificantChange_Generic_YesNoNa? landTransactionConsent,
	string? landTransactionConsentAdditionalInfo,
	string? landTransactionSupportingEvidence)
	: SetSignificantChangeLandTransactionPublicCommand(landTransactionApplication, landTransactionApplicationAdditionalInfo, landTransactionConsent, landTransactionConsentAdditionalInfo, landTransactionSupportingEvidence)
{
	public int Id { get; set; } = id;
}