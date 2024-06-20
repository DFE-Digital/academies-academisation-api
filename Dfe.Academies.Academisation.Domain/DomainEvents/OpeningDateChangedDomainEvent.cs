using MediatR;

public class OpeningDateChangedDomainEvent : INotification
{
	public int EntityId { get; }
	public string EntityType { get; }
	public DateTime? OldDate { get; }
	public DateTime? NewDate { get; }
	public DateTime ChangedAt { get; }

	public OpeningDateChangedDomainEvent(int entityId, string entityType, DateTime? oldDate, DateTime? newDate, DateTime changedAt)
	{
		EntityId = entityId;
		EntityType = entityType;
		OldDate = oldDate;
		NewDate = newDate;
		ChangedAt = changedAt;
	}
}
