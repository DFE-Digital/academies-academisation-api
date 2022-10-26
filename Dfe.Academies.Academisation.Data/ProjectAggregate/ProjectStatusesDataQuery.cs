using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class ProjectStatusesDataQuery : IProjectStatusesDataQuery
	{
		private readonly AcademisationContext _context;

		public ProjectStatusesDataQuery(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<List<string?>> Execute()
		{
			return await _context.Projects
				.AsNoTracking()
				.Select(p => p.ProjectStatus)
				.Distinct()
				.OrderBy(p => p)
				.ToListAsync();
		}
	}
}
