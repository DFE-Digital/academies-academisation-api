using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	public class ApplicationGetDataQuery : IApplicationGetDataQuery
	{
		private readonly AcademisationContext _context;

		public ApplicationGetDataQuery(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<IApplication?> Execute(int id)
		{
			var applicationState = await _context.Applications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.Include(a => a.Schools)
				.SingleOrDefaultAsync(a => a.Id == id);

			return applicationState?.MapToDomain();
		}
	}
}
