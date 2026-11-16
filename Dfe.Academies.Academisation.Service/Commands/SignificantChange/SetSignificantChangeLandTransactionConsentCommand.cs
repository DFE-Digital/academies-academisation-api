using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeLandTransactionPublicCommand(
	YesNoNotApplicableAnswer? landTransactionApplication,
	string? landTransactionApplicationAdditionalInfo,
	YesNoNotApplicableAnswer? landTransactionConsent,
	string? landTransactionConsentAdditionalInfo,
	string? landTransactionSupportingEvidenceSharepointLink) : IRequest<CommandResult>
{
	public YesNoNotApplicableAnswer? LandTransactionApplication { get; set; } = landTransactionApplication;
	public string? LandTransactionApplicationAdditionalInfo { get; set; } = landTransactionApplicationAdditionalInfo;
	public YesNoNotApplicableAnswer? LandTransactionConsent { get; set; } = landTransactionConsent;
	public string? LandTransactionConsentAdditionalInfo { get; set; } = landTransactionConsentAdditionalInfo;
	public string? LandTransactionSupportingEvidenceSharepointLink { get; set; } = landTransactionSupportingEvidenceSharepointLink;
}

public class SetSignificantChangeLandTransactionCommand(
	int id,
	YesNoNotApplicableAnswer? landTransactionApplication,
	string? landTransactionApplicationAdditionalInfo,
	YesNoNotApplicableAnswer? landTransactionConsent,
	string? landTransactionConsentAdditionalInfo,
	string? landTransactionSupportingEvidenceSharepointLink)
	: SetSignificantChangeLandTransactionPublicCommand(landTransactionApplication, landTransactionApplicationAdditionalInfo, landTransactionConsent, landTransactionConsentAdditionalInfo, landTransactionSupportingEvidenceSharepointLink)
{
	public int Id { get; set; } = id;
}