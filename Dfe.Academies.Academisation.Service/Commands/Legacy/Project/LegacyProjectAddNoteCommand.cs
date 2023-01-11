using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class LegacyProjectAddNoteCommand : ILegacyProjectAddNoteCommand
	{
		private readonly IProjectGetDataQuery _projectGetDataQuery;
		private readonly AcademisationContext _context;

		public LegacyProjectAddNoteCommand(IProjectGetDataQuery projectGetDataQuery, AcademisationContext context)
		{
			_projectGetDataQuery = projectGetDataQuery;
			_context = context;
		}

		public async Task<CommandResult> Execute(LegacyProjectAddNoteModel model)
		{
			IProject? project = await _projectGetDataQuery.Execute(model.ProjectId);

			if (project is null) return new NotFoundCommandResult();

			_context.ProjectNotes.Add(new ProjectNoteState
			{
				Author = model.Author,
				Date = model.Date,
				Subject = model.Subject,
				Note = model.Note,
				ProjectId = model.ProjectId
			});

			await _context.SaveChangesAsync();

			return new CommandSuccessResult();
		}
	}
}
