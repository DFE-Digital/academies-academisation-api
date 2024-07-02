using MediatR;

public abstract class Entity
{
	private List<INotification> _domainEvents;
	public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

	public int Id { get; set; }
	public DateTime CreatedOn { get; set; }
	public DateTime LastModifiedOn { get; set; }

	protected void AddDomainEvent(INotification eventItem)
	{
		_domainEvents = _domainEvents ?? new List<INotification>();
		_domainEvents.Add(eventItem);
	}

	public void ClearDomainEvents()
	{
		_domainEvents?.Clear();
	}
}
