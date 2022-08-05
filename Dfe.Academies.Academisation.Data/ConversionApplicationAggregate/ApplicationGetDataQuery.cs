using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate
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
			var conversionApplicationState = await _context.ConversionApplications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.SingleOrDefaultAsync(a => a.Id == id);

			return conversionApplicationState?.MapToDomain();
		}
	}
}
