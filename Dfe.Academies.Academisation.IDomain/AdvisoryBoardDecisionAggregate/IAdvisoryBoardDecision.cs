namespace Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;

public interface IAdvisoryBoardDecision
{
    int Id { get; set; }
    int ProjectId { get; }
    IAdvisoryBoardDecisionDetails Details { get; }
}