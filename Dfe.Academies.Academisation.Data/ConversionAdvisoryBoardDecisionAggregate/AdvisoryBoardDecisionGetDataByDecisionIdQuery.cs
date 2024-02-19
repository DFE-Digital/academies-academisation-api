using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionGetDataByDecisionIdQuery : IAdvisoryBoardDecisionGetDataByDecisionIdQuery
{
	private readonly AcademisationContext _context;

	public AdvisoryBoardDecisionGetDataByDecisionIdQuery(AcademisationContext context)
	{
		_context = context;
	}

	public async Task<IConversionAdvisoryBoardDecision?> Execute(int decisionId)
	{
		var state = await _context.ConversionAdvisoryBoardDecisions
			.Include(s => s.DeclinedReasons)
			.Include(s => s.DeferredReasons)
			.Include(s => s.WithdrawnReasons)
			.FirstOrDefaultAsync(s => s.Id == decisionId);

		return state?.MapToDomain();
	}
}
