using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;

public class AdvisoryBoardDecisionFactory : IAdvisoryBoardDecisionFactory
{
    public async Task<IAdvisoryBoardDecision> Create(int projectId, IAdvisoryBoardDecisionDetails details)
    {
        return await AdvisoryBoardDecision.Create(projectId, details);
    }
}