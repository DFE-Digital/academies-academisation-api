using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetProjectDatesCommand(int id, DateTime? advisoryBoardDate, DateTime? previousAdvisoryBoard,
		DateTime? proposedConversionDate, List<ReasonChange>? reasonsChanged, string? changedBy,
		bool? projectDatesSectionComplete) : IRequest<CommandResult>
	{
		public int Id { get; set; } = id;
		public DateTime? AdvisoryBoardDate { get; set; } = advisoryBoardDate;
		public DateTime? PreviousAdvisoryBoard { get; set; } = previousAdvisoryBoard;
		public DateTime? ProposedConversionDate { get; set; } = proposedConversionDate;
		public List<ReasonChange>? ReasonsChanged { get; set; } = reasonsChanged;
		public string? ChangedBy { get; set; } = changedBy;
		public bool? ProjectDatesSectionComplete { get; set; } = projectDatesSectionComplete;
	}
}


