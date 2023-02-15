using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

public class ProjectUpdateDataCommand : IProjectUpdateDataCommand
{
	private readonly AcademisationContext _context;

	public ProjectUpdateDataCommand(AcademisationContext context)
	{
		_context = context;
	}

	public async Task Execute(IProject project)
	{
		var projectState = ProjectState.MapFromDomain(project);

		var projectInDb = await _context.Projects.SingleAsync(p => p.Id == project.Id);
		projectState.CreatedOn = projectInDb.CreatedOn;

		_context.ReplaceTracked(projectState)
			.Collection(x => x.Notes!).IsModified = false;

		await _context.SaveChangesAsync();
	}
}
