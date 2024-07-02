using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetProjectDatesCommand : IRequest<CommandResult>
	{
		public SetProjectDatesCommand(int id, DateTime? advisoryBoardDate, DateTime? previousAdvisoryBoard, DateTime? proposedConversionDate, List<ReasonChange>? reasonsChanged, string? changedBy, bool? projectDatesSectionComplete)
		{
			Id = id;
			AdvisoryBoardDate = advisoryBoardDate;
			PreviousAdvisoryBoard = previousAdvisoryBoard;
			ProposedConversionDate = proposedConversionDate;
			ReasonsChanged = reasonsChanged;
			ChangedBy = changedBy;
			ProjectDatesSectionComplete = projectDatesSectionComplete;
		}

		public int Id { get; set; }
		public DateTime? AdvisoryBoardDate { get; set; }
		public DateTime? PreviousAdvisoryBoard { get; set; }
		public DateTime? ProposedConversionDate { get; set; }
		public List<ReasonChange>? ReasonsChanged { get; set; }
		public string? ChangedBy { get; set; }
		public bool? ProjectDatesSectionComplete { get; set; }
	}
}


