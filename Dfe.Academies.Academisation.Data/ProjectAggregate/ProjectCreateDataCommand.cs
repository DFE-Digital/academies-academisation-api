using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

public class ProjectCreateDataCommand : IProjectCreateDataCommand
{
	private readonly AcademisationContext _context;

	public ProjectCreateDataCommand(AcademisationContext context)
	{
		_context = context;
	}

	public async Task<IProject> Execute(IProject project)
	{
		var projectState = ProjectState.MapFromDomain(project);

		_context.Projects.Add(projectState);
		await _context.SaveChangesAsync();

		_context.ChangeTracker.Clear();
		ProjectState createdProjectState = await _context.Projects.SingleAsync(a => a.Id == projectState.Id);

		return createdProjectState.MapToDomain();
	}
}
