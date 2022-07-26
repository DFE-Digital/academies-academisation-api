using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionFactory : IConversionAdvisoryBoardDecisionFactory
{
	public async Task<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details)
	{
		return await ConversionAdvisoryBoardDecision.Create(details);
	}
}
