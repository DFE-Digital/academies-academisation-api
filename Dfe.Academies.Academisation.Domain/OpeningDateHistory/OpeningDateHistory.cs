namespace Dfe.Academies.Academisation.Domain.OpeningDateHistory
{
	public class OpeningDateHistory
	{
		public int Id { get; set; }
		public int EntityId { get; set; }
		public string EntityType { get; set; }
		public DateTime? OldDate { get; set; }
		public DateTime? NewDate { get; set; }
		public DateTime ChangedAt { get; set; }
	}

}
