using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate;

public class ProjectGetDataQuery : IProjectGetDataQuery
{
	private readonly AcademisationContext _context;

	public ProjectGetDataQuery(AcademisationContext context)
	{
		_context = context;
	}

	public async Task<IProject?> Execute(int id)
	{
		ProjectState createdProjectState = await _context.Projects.SingleAsync(a => a.Id == id);

		return createdProjectState.MapToDomain();
	}
}
