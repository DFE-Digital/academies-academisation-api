using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeProjectDatesPublicCommand(
		DateTime? ProposedDecisionDate,
		DateTime? ProposedChangeDate);

	public record SetSignificantChangeProjectDatesCommand(
		int Id,
		DateTime? ProposedDecisionDate,
		DateTime? ProposedChangeDate)
		: SetSignificantChangeProjectDatesPublicCommand(ProposedDecisionDate,
			ProposedChangeDate), IRequest<CommandResult>;
}
