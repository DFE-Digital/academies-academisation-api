using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject
{
	public class ConversionProjectDeleteNoteCommandHandler : IRequestHandler<ConversionProjectDeleteNoteCommand, CommandResult>
	{
		private readonly IProjectNoteDeleteCommand _deleteNoteCommand;

		public ConversionProjectDeleteNoteCommandHandler(IProjectNoteDeleteCommand deleteNoteCommand)
		{
			_deleteNoteCommand = deleteNoteCommand;
		}

		public async Task<CommandResult> Handle(ConversionProjectDeleteNoteCommand note, CancellationToken cancellationToken)
		{
			return await _deleteNoteCommand.Execute(
				note.ProjectId,
				new ProjectNote(
					note.Subject,
					note.Note,
					note.Author,
					note.Date, note.ProjectId)
			);
		}
	}
}
