using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLandTransactionConsentPublicCommand(
	SignificantChangeLandTransactionConsent? landTransactionConsentSecured,
	string? landTransactionConsentAdditionalInfo) : IRequest<CommandResult>
{
	public SignificantChangeLandTransactionConsent? LandTransactionConsentSecured { get; set; } = landTransactionConsentSecured;
	public string? LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;
}

public class SetSignificantChangeLandTransactionConsentCommand(
	int id,
	SignificantChangeLandTransactionConsent? landTransactionConsentSecured,
	string? landTransactionConsentAdditionalInfo)
	: SetSignificantChangeLandTransactionConsentPublicCommand(landTransactionConsentSecured, landTransactionConsentAdditionalInfo)
{
	public int Id { get; set; } = id;
}