using Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.AdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionFactory : IAdvisoryBoardDecisionFactory
{
    public async Task<IAdvisoryBoardDecision> Create(IAdvisoryBoardDecisionDetails details)
    {
        return await AdvisoryBoardDecision.Create(details);
    }
}