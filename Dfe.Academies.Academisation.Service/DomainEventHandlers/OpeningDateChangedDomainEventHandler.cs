using Dfe.Academies.Academisation.Data;
using MediatR;

public class OpeningDateChangedDomainEventHandler : INotificationHandler<OpeningDateChangedDomainEvent>
{
	private readonly AcademisationContext _context;

	public OpeningDateChangedDomainEventHandler(AcademisationContext context)
	{
		_context = context;
	}

	public async Task Handle(OpeningDateChangedDomainEvent notification, CancellationToken cancellationToken)
	{
		var historyRecord = new OpeningDateHistory
		{
			EntityId = notification.EntityId,
			EntityType = notification.EntityType,
			OldDate = notification.OldDate,
			NewDate = notification.NewDate,
			ChangedAt = notification.ChangedAt,
			ChangedBy = notification.ChangedBy,
			ReasonsChanged = notification.ReasonsChanged
		};

		_context.OpeningDateHistories.Add(historyRecord);
		await _context.SaveChangesAsync(cancellationToken);
	}
}
