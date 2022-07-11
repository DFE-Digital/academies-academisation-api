namespace Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecision
{
    int Id { get; set; }
    IAdvisoryBoardDecisionDetails Details { get; }
}