using Dfe.Academies.Academisation.Domain.ProjectAggregate;
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
		//var projectState = Project.MapFromDomain(project);

		var projectInDb = await _context.Projects.SingleAsync(p => p.Id == project.Id);
		project.Details.CreatedOn = projectInDb.Details.CreatedOn;

		_context.ReplaceTracked(project as Project)
			.Collection(x => x.Details.Notes!).IsModified = false;

		await _context.SaveChangesAsync();
	}
}
