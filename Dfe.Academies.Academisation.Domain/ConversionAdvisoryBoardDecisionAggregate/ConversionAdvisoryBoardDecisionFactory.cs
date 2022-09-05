using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionFactory : IConversionAdvisoryBoardDecisionFactory
{
	public CreateResult<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details)
	{
		return ConversionAdvisoryBoardDecision.Create(details);
	}
}
