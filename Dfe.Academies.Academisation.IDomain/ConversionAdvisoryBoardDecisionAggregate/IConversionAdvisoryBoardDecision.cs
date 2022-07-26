using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

public interface IConversionAdvisoryBoardDecision
{
	int Id { get; set; }
	AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; }	
}
