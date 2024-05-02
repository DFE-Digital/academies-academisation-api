using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

public interface IConversionAdvisoryBoardDecision
{
	int Id { get; }
	DateTime CreatedOn { get; }
	DateTime LastModifiedOn { get; }
	AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; }
	void SetId(int id);
	CommandResult Update(AdvisoryBoardDecisionDetails details, 
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons);

	public IReadOnlyCollection<AdvisoryBoardDeclinedReasonDetails> DeclinedReasons { get; }
	public IReadOnlyCollection<AdvisoryBoardDeferredReasonDetails> DeferredReasons { get; }
	public IReadOnlyCollection<AdvisoryBoardWithdrawnReasonDetails> WithdrawnReasons { get; }


}
