using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext
{
	public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options) { }	
	public DbSet<ApplicationState> Applications { get; set; } = null!;
	public DbSet<ContributorState> Contributors { get; set; } = null!;
	public DbSet<ApplicationSchoolState> Schools { get; set; } = null!;

	public DbSet<ConversionAdvisoryBoardDecisionState> ConversionAdvisoryBoardDecisions { get; set; } = null!;

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
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

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("academisation");
		modelBuilder.Entity<ApplicationState>()
			.Property(e => e.ApplicationType)
			.HasConversion<string>();

		modelBuilder.Entity<ApplicationState>()
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
