namespace Dfe.Academies.Academisation.Domain.SeedWork
{
	public abstract class Entity
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime LastModifiedOn { get; set; }
	}
}
