namespace Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

public interface IAdvisoryBoardDecisionFactory
{
    Task<IAdvisoryBoardDecision> Create(int projectId, IAdvisoryBoardDecisionDetails details);
}