using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

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
			ConversionApplicationState conversionApplicationState =  await _context.ConversionApplications.FindAsync(id) ?? throw new ArgumentException($"Entity with value {id} not found");

			return conversionApplicationState.MapToDomain();
		}
	}
}
