using Dfe.Academies.Academisation.Data.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext
{
	
	private const string ConnectionStringName = "SQLAZURECONNSTR_ConnectionString";
	private const string ConfigurationMissing = "Could not retrieve connection string from configuration";
	
	public virtual DbSet<ConversionApplicationState> ConversionApplications { get; set; }
	public virtual DbSet<ConversionApplicationContributorState> Contributors { get; set; }
	
	// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	// {
	// 	if (optionsBuilder.IsConfigured) return;
 //        
	// 	var connectionString = Environment.GetEnvironmentVariable(ConnectionStringName) 
	// 	                       ?? throw new ApplicationException(ConfigurationMissing);
	// 	optionsBuilder.UseSqlServer(connectionString);
	// }
	
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
