using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class LegacyProjectDeleteNoteCommand : ILegacyProjectDeleteNoteCommand
	{
		private readonly AcademisationContext _context;

		public LegacyProjectDeleteNoteCommand(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<CommandResult> Execute(int projectId, ProjectNoteServiceModel note)
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
