using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class ProjectListGetDataQuery : IProjectListGetDataQuery
	{
		private readonly AcademisationContext _context;

		public ProjectListGetDataQuery(AcademisationContext context)
		{
			_context = context;
		}
		
		public async Task<IEnumerable<IProject>> SearchProjects(List<string>? states, int page, int count, int? urn)
		{
			IQueryable<ProjectState> queryable = _context.Projects
				.OrderByDescending(acp => acp.ApplicationReceivedDate);
			
			if (states != null && states!.Any())
			{
				// var lowerStatuses = states.Select(status => status.ToLower());
				queryable = queryable.Where(p => states.Contains(p.ProjectStatus!.ToLower()));
			}

			if (urn.HasValue)
			{
				queryable = queryable.Where(acp => acp.Urn == urn);
			}

			var projects =  await queryable
				.Skip((page - 1) * count)
				.Take(count).ToListAsync();
			
			return projects.Select(p => p.MapToDomain());
		}
	}
}
