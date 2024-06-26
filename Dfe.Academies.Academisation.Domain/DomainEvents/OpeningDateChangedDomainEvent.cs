using Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate;
using MediatR;

public class OpeningDateChangedDomainEvent : INotification
{
	public int EntityId { get; }
	public string EntityType { get; }
	public DateTime? OldDate { get; }
	public DateTime? NewDate { get; }
	public DateTime ChangedAt { get; }
	public string ChangedBy { get; }
	public List<ReasonChange> ReasonsChanged { get; }

	public OpeningDateChangedDomainEvent(int entityId, string entityType, DateTime? oldDate, DateTime? newDate, DateTime changedAt, string changedBy, List<ReasonChange> reasonsChanged)
	{
		EntityId = entityId;
		EntityType = entityType;
		OldDate = oldDate;
		NewDate = newDate;
		ChangedAt = changedAt;
		ChangedBy = changedBy;
		ReasonsChanged = reasonsChanged;
	}
}
