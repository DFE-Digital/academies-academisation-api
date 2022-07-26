using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

public interface IConversionAdvisoryBoardDecisionFactory
{
	Task<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details);
}
