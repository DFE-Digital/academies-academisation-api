using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class LegacyProjectDeleteNoteCommand : ILegacyProjectDeleteNoteCommand
	{
		private readonly IProjectNoteDeleteCommand _deleteNoteCommand;

		public LegacyProjectDeleteNoteCommand(IProjectNoteDeleteCommand deleteNoteCommand)
		{
			_deleteNoteCommand = deleteNoteCommand;
		}

		public async Task<CommandResult> Execute(int projectId, ProjectNoteServiceModel note)
		{
			return await _deleteNoteCommand.Execute(
				projectId,
				new ProjectNote(
					note.Subject,
					note.Note,
					note.Author,
					note.Date)
			);
		}
	}
}
