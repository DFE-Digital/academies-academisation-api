using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

public interface IConversionAdvisoryBoardDecision
{
	public int Id { get; }
	AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; }

	void SetId(int id);
}
