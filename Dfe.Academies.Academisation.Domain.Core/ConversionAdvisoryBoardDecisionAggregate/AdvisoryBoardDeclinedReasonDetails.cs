namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDeclinedReasonDetails(int AdvisoryBoardDecisionId, AdvisoryBoardDeclinedReason Reason, string Details)
{
	public int Id { get; private set; }
	public DateTime CreatedOn { get; set; }
	public DateTime LastModifiedOn { get; set; }

}
