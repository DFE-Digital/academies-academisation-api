using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public record SetSignificantChangeFhaRisksOrIssuesRaisedPublicCommand(
	bool? FhaRisksOrIssuesRaised,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink);

public record SetSignificantChangeFhaRisksOrIssuesRaisedCommand(
	int Id,
	bool? FhaRisksOrIssuesRaised,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink)
	: SetSignificantChangeFhaRisksOrIssuesRaisedPublicCommand(FhaRisksOrIssuesRaised, AdditionalInformation, SupportingEvidenceSharepointLink),
		IRequest<CommandResult>;
