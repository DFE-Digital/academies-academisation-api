using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionUpdateDataCommand : IAdvisoryBoardDecisionUpdateDataCommand
{
	private readonly AcademisationContext _context;

	public AdvisoryBoardDecisionUpdateDataCommand(AcademisationContext context)
	{
		_context = context;
	}
	
	public async Task Execute(IConversionAdvisoryBoardDecision decision)
	{
		var decisionState = ConversionAdvisoryBoardDecisionState.MapFromDomain(decision);
		
		var existingState = await _context.ConversionAdvisoryBoardDecisions
			.AsNoTracking()
			.SingleOrDefaultAsync(d => d.Id == decision.Id);

		if (existingState is null)
		{
			_context.Add(decisionState);
		}
		else
		{
			decisionState.CreatedOn = existingState.CreatedOn;
			_context.Update(decisionState);
		}
	
		await _context.SaveChangesAsync();
	}
}
