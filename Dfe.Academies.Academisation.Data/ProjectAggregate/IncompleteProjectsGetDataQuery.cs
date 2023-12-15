using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class IncompleteProjectsGetDataQuery : IIncompleteProjectsGetDataQuery
	{
		private readonly AcademisationContext _context;

		public IncompleteProjectsGetDataQuery(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<IProject>?> GetIncompleteProjects()
		{
			var createdProjectState = await _context.Projects
				.Where(p => string.IsNullOrEmpty(p.Details.LocalAuthority) || string.IsNullOrEmpty(p.Details.Region) || string.IsNullOrEmpty(p.Details.SchoolPhase) || string.IsNullOrEmpty(p.Details.SchoolType))
				.ToListAsync();

			return createdProjectState;
		}
	}
}
