using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public record SetSignificantChangeFhaRisksOrIssuesRaisedPublicCommand(
	bool? FhaRisksOrIssuesRaised,
	string? AdditionalInformation,
	string? SupportingEvidence);

public record SetSignificantChangeFhaRisksOrIssuesRaisedCommand(
	int Id,
	bool? FhaRisksOrIssuesRaised,
	string? AdditionalInformation,
	string? SupportingEvidence)
	: SetSignificantChangeFhaRisksOrIssuesRaisedPublicCommand(FhaRisksOrIssuesRaised, AdditionalInformation, SupportingEvidence),
		IRequest<CommandResult>;
