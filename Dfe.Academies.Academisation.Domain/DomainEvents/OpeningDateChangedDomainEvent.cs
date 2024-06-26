using MediatR;

public class OpeningDateChangedDomainEvent : INotification
{
	public int EntityId { get; }
	public string EntityType { get; }
	public DateTime? OldDate { get; }
	public DateTime? NewDate { get; }
	public DateTime ChangedAt { get; }
	public string ChangedBy { get; set; }
	public string ReasonForChange { get; set; }
	public string ReasonForChangeDetails { get; set; }

	public OpeningDateChangedDomainEvent(int entityId, string entityType, DateTime? oldDate, DateTime? newDate, DateTime changedAt, string changedBy, string reasonForChange, string reasonForChangeDetails)
	{
		EntityId = entityId;
		EntityType = entityType;
		OldDate = oldDate;
		NewDate = newDate;
		ChangedAt = changedAt;
		ChangedBy = changedBy;
		ReasonForChange = reasonForChange;
		ReasonForChangeDetails = reasonForChangeDetails;
	}
}
