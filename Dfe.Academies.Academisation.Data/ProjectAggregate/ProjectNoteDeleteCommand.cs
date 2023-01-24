using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class ProjectNoteDeleteCommand : IProjectNoteDeleteCommand
	{
		private readonly AcademisationContext _context;

		public ProjectNoteDeleteCommand(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<CommandResult> Execute(int projectId, ProjectNote note)
		{
			ProjectNoteState? matchedNote =
				await _context.ProjectNotes
					.FirstOrDefaultAsync(x => x.ProjectId == projectId &&
											  x.Subject == note.Subject &&
											  x.Note == note.Note &&
											  x.Author == note.Author &&
											  x.Date == note.Date);

			if (matchedNote is null)
			{
				return new NotFoundCommandResult();
			}

			_context.ProjectNotes.Remove(matchedNote);
			await _context.SaveChangesAsync();

			return new CommandSuccessResult();
		}
	}
}
