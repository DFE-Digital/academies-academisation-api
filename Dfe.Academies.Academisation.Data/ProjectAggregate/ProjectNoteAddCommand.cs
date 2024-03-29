﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class ProjectNoteAddCommand : IProjectNoteAddCommand
	{
		private readonly AcademisationContext _context;

		public ProjectNoteAddCommand(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<CommandResult> Execute(int projectId, ProjectNote note)
		{
			if (_context.Projects.Any(x => x.Id == projectId) is false)
			{
				return new NotFoundCommandResult();
			}

			_context.ProjectNotes.Add(new ProjectNote (note.Subject, note.Note, note.Author,note.Date, projectId));

			await _context.SaveChangesAsync();

			return new CommandSuccessResult();
		}
	}
}
