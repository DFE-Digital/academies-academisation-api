using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

public interface IConversionAdvisoryBoardDecisionFactory
{
	CreateResult<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details);
}
