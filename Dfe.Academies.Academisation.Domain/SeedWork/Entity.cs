namespace Dfe.Academies.Academisation.Domain.SeedWork
{
	public abstract class Entity
	{
		public virtual int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime LastModifiedOn { get; set; }
	}
}
