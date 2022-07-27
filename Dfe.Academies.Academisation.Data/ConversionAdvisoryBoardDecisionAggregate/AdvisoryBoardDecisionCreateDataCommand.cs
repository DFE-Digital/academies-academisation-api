﻿using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionCreateDataCommand : IAdvisoryBoardDecisionCreateDataCommand
{
    private readonly AcademisationContext _context;

    public AdvisoryBoardDecisionCreateDataCommand(AcademisationContext context)
    {
        _context = context;
    }

    public async Task<int> Execute(IConversionAdvisoryBoardDecision decision)
    {
        var decisionState = ConversionAdvisoryBoardDecisionState.MapFromDomain(decision);

        await _context.ConversionAdvisoryBoardDecisionStates.AddAsync(decisionState);
        await _context.SaveChangesAsync();

        return decisionState.Id;

    }
}
