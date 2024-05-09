using Dfe.Academies.Academisation.Domain.ProjectAggregate;
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

		_context.Entry(project as Project).State = EntityState.Modified;
		_context.Update(project as Project);

		await _context.SaveChangesAsync();
	}
}
