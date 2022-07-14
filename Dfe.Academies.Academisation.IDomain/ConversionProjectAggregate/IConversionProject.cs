namespace Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

public interface IConversionProject
{
    int Id { get; set; }
    IAdvisoryBoardDecision? AdvisoryBoardDecision { get; }
    Task AddAdvisoryBoardDecision(IAdvisoryBoardDecisionDetails details);
}