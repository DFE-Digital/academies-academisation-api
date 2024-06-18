using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetProjectDatesCommand : IRequest<CommandResult>
	{
		public SetProjectDatesCommand(int id, DateTime? advisoryBoardDate, DateTime? previousAdvisoryBoard)
		{
			Id = id;
			AdvisoryBoardDate = advisoryBoardDate;
			PreviousAdvisoryBoard = previousAdvisoryBoard;
		}

		public int Id { get; set; }
		public DateTime? AdvisoryBoardDate { get; set; }
		public DateTime? PreviousAdvisoryBoard { get; set; }
	}
}


