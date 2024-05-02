namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDeferredReasonDetails(int AdvisoryBoardDecisionId, AdvisoryBoardDeferredReason Reason, string Details)
{
	public int Id { get; private set; }
	public DateTime CreatedOn { get; set; }
	public DateTime LastModifiedOn { get; set; }
}
