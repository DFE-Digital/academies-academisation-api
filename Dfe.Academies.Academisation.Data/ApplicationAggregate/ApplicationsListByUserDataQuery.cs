using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	public class ApplicationsListByUserDataQuery : IApplicationsListByUserDataQuery
	{
		private readonly AcademisationContext _context;
		
		public ApplicationsListByUserDataQuery(AcademisationContext context)
		{
			_context = context;
		}
		
		public async Task<IList<IApplication>> Execute(string userEmail)
		{
			List<ApplicationState> applicationStates = await _context.Applications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.Include(a => a.Schools)
				.Where(a => a.Contributors.Any(c => c.EmailAddress == userEmail))
				.ToListAsync();

			return applicationStates.Select(a => a.MapToDomain()).ToList();
		}
	}
}
