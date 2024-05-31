using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionFactory : IConversionAdvisoryBoardDecisionFactory
{
	public CreateResult Create(AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons,
		IEnumerable<AdvisoryBoardDAORevokedReasonDetails> daoRevokedReasons)
	{
		return ConversionAdvisoryBoardDecision.Create(details, deferredReasons, declinedReasons, withdrawnReasons, daoRevokedReasons);
	}

}
