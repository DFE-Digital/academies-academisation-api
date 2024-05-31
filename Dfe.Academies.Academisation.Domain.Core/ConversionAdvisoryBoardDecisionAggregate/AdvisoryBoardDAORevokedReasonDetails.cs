namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDAORevokedReasonDetails(int AdvisoryBoardDecisionId, AdvisoryBoardDAORevokedReason Reason, string Details)
{
	public int Id { get; private set; }
	public DateTime CreatedOn { get; set; }
	public DateTime LastModifiedOn { get; set; }

}
