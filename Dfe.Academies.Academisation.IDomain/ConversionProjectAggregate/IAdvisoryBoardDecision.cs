namespace Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

public interface IAdvisoryBoardDecision
{
	int Id { get; set; }
	int ProjectId { get; }
	IAdvisoryBoardDecisionDetails Details { get; }
}