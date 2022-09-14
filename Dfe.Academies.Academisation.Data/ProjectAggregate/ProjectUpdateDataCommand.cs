using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

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

		_context.ReplaceTracked(projectState);
		
		await _context.SaveChangesAsync();
	}
}
