using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.DomainEventHandlers
{
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
				ChangedBy = notification.ChangedBy
			};

			_context.OpeningDateHistories.Add(historyRecord);
			await _context.SaveChangesAsync(cancellationToken);
		}
	}

}
