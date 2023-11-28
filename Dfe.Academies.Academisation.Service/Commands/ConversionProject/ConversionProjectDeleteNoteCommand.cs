using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectDeleteNoteCommand : IRequest<CommandResult>
	{
		public int ProjectId { get; set; }
		public string? Subject { get; set; } = null;
		public string? Note { get; set; } = null;
		public string? Author { get; set; } = null;
		public DateTime? Date { get; set; } = null;
	}
}
