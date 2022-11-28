using Dfe.Academies.Academisation.Domain.ProjectAggregate;
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

		public async Task<ProjectFilterParameters> Execute()
		{
			ProjectFilterParameters filterParameters = new ProjectFilterParameters
			{
				Statuses = await _context.Projects
				.AsNoTracking()
				.Select(p => p.ProjectStatus)
				.Distinct()
				.OrderBy(p => p)
				.ToListAsync(),
				AssignedUsers = await _context.Projects
					.OrderByDescending(p => p.AssignedUserFullName)
					.AsNoTracking()
					.Select(p => p.AssignedUserFullName)
					.Where(p => !string.IsNullOrEmpty(p))
					.Distinct()
					.ToListAsync()
			};

			return filterParameters;
		}
	}
}
