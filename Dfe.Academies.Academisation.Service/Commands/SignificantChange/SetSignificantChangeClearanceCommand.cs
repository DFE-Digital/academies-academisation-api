using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public record SetSignificantChangeClearancePublicCommand(
	string? RegionsGroupClearance,
	string? TrustRegulationClearance);

public record SetSignificantChangeClearanceCommand(
	int Id,
	string? RegionsGroupClearance,
	string? TrustRegulationClearance)
	: SetSignificantChangeClearancePublicCommand(RegionsGroupClearance, TrustRegulationClearance), IRequest<CommandResult>;
