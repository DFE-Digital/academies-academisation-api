namespace Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecisionFactory
{
    Task<IAdvisoryBoardDecision> Create(int projectId, IAdvisoryBoardDecisionDetails details);
}