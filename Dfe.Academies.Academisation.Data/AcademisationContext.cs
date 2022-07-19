using Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Data.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext
{
	public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options) { }
	
	public DbSet<ConversionApplicationState> ConversionApplications { get; set; } = null!;
	public DbSet<ContributorState> Contributors { get; set; } = null!;

	public override int SaveChanges() 
	{
		var currentDateTime = DateTime.UtcNow;
		
		var entities = ChangeTracker.Entries<BaseEntity>().ToList();
		
		foreach (var entity in entities.Where(e => e.State == EntityState.Added)) 
		{
			entity.Entity.CreatedOn = currentDateTime;
			entity.Entity.LastModifiedOn = currentDateTime;
		}

		foreach (var entity in entities.Where(e => e.State == EntityState.Modified))
		{
			entity.Entity.LastModifiedOn = currentDateTime;
		}

		return base.SaveChanges();
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ConversionApplicationState>()
			.HasEnum(e => e.ApplicationType);
		
		base.OnModelCreating(modelBuilder);
	}
}
