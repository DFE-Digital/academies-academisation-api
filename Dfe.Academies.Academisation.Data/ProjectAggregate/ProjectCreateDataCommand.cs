using Dfe.Academies.Academisation.Domain.ProjectAggregate;
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
		
		_context.Projects.Add(project as Project);
		await _context.SaveChangesAsync();

		_context.ChangeTracker.Clear();
		Project createdProjectState = await _context.Projects.SingleAsync(a => a.Id == project.Id);

		return createdProjectState;

	}
}
