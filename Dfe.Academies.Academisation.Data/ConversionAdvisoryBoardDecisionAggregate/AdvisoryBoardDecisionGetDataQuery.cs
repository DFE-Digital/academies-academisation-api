using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate
{
	public class AdvisoryBoardDecisionGetDataQuery : IAdvisoryBoardDecisionGetDataQuery
	{
		private readonly AcademisationContext _context;

		public AdvisoryBoardDecisionGetDataQuery(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<IConversionAdvisoryBoardDecision?> Execute(int id)
		{
			var state = await _context.ConversionAdvisoryBoardDecisions
				.Include(s => s.DeclinedReasons)
				.Include(s => s.DeferredReasons)
				.AsNoTracking()
				.SingleOrDefaultAsync(s => s.Id == id);
			
			return state?.MapToDomain();
		}
	}
}
