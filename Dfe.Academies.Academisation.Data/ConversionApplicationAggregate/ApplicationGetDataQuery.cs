using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
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

		public async Task<IConversionApplication> Execute(int id)
		{
			var conversionApplicationState = await _context.ConversionApplications
				.AsNoTracking()
				.Include(a => a.Contributors)
				.SingleAsync(a => a.Id == id);

			return conversionApplicationState.MapToDomain();
		}
	}
}
