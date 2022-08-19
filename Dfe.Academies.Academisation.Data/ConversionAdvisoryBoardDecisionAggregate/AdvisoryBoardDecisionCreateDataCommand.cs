using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionCreateDataCommand : IAdvisoryBoardDecisionCreateDataCommand
{
	private readonly AcademisationContext _context;

	public AdvisoryBoardDecisionCreateDataCommand(AcademisationContext context)
	{
		_context = context;
	}

	public async Task Execute(IConversionAdvisoryBoardDecision decision)
	{
		var decisionState = ConversionAdvisoryBoardDecisionState.MapFromDomain(decision);

		await _context.ConversionAdvisoryBoardDecisions.AddAsync(decisionState);
		await _context.SaveChangesAsync();

		decision.SetId(decisionState.Id);
	}
}
