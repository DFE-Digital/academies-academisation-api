using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate
{
	public class OpeningDateHistory : Entity, IAggregateRoot
	{
		public int Id { get; set; }
		public int EntityId { get; set; }
		public string EntityType { get; set; }
		public DateTime? OldDate { get; set; }
		public DateTime? NewDate { get; set; }
		public DateTime ChangedAt { get; set; }
		public string? ChangedBy { get; set; }
		public List<ReasonChange>? ReasonsChanged { get; set; }


	}
}
