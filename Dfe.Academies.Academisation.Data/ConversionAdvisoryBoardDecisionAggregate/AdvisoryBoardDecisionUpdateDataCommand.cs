﻿using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

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
		var decisionState = AdvisoryBoardDecisionState.MapFromDomain(decision);

		_context.ReplaceTracked(decisionState);

		await _context.SaveChangesAsync();
	}
}
