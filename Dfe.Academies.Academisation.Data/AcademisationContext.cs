using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext
{
	public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options) { }	
	public DbSet<ConversionApplicationState> ConversionApplications { get; set; } = null!;
	public DbSet<ContributorState> Contributors { get; set; } = null!;
	public DbSet<ConversionAdvisoryBoardDecisionState> ConversionAdvisoryBoardDecisions { get; set; } = null!;

	public override int SaveChanges()
	{
		SetModifiedAndCreatedDates();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
	{
		SetModifiedAndCreatedDates();
		return base.SaveChangesAsync(cancellationToken);
	}

	private void SetModifiedAndCreatedDates()
	{
		var timestamp = DateTime.UtcNow;

		var entities = ChangeTracker.Entries<BaseEntity>().ToList();

		foreach (var entity in entities.Where(e => e.State == EntityState.Added))
		{
			entity.Entity.CreatedOn = timestamp;
			entity.Entity.LastModifiedOn = timestamp;
		}

		foreach (var entity in entities.Where(e => e.State == EntityState.Modified))
		{
			entity.Entity.LastModifiedOn = timestamp;
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("academisation");
		modelBuilder.Entity<ConversionApplicationState>()
			.Property(e => e.ApplicationType)
			.HasConversion<string>();

		modelBuilder.Entity<ConversionApplicationState>()
			.Property(e => e.ApplicationStatus)
			.HasConversion<string>();

		OnAdvisoryBoardDecisionCreating(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	private static void OnAdvisoryBoardDecisionCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ConversionAdvisoryBoardDecisionState>()
			.Property(e => e.DecisionMadeBy)
			.HasConversion<string>();

		modelBuilder.Entity<ConversionAdvisoryBoardDecisionDeclinedReasonState>()
			.Property(e => e.Reason)
			.HasConversion<string>();

		modelBuilder.Entity<ConversionAdvisoryBoardDecisionDeferredReasonState>()
			.Property(e => e.Reason)
			.HasConversion<string>();

		modelBuilder.Entity<ConversionAdvisoryBoardDecisionState>()
			.Property(e => e.Decision)
			.HasConversion<string>();
	}
}
