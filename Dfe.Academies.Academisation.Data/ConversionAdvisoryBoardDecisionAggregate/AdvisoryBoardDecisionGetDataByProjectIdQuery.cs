using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionGetDataByProjectIdQuery : IAdvisoryBoardDecisionGetDataByProjectIdQuery
{
	private readonly AcademisationContext _context;

	public AdvisoryBoardDecisionGetDataByProjectIdQuery(AcademisationContext context)
	{
		_context = context;
	}

	public async Task<IConversionAdvisoryBoardDecision?> Execute(int projectId)
	{
		var state = await _context.ConversionAdvisoryBoardDecisions
			.Include(s => s.DeclinedReasons)
			.Include(s => s.DeferredReasons)
			.AsNoTracking()
			.SingleOrDefaultAsync(s => s.ConversionProjectId == projectId);
			
		return state?.MapToDomain();
	}
}
