﻿using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.ConversionLegalRequirement;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext
{
	public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options) { }

	public DbSet<LegalRequirementState> LegalRequirements { get; set; } = null!;
	public DbSet<ApplicationState> Applications { get; set; } = null!;
	public DbSet<ContributorState> Contributors { get; set; } = null!;
	public DbSet<ApplicationSchoolState> Schools { get; set; } = null!;
	public DbSet<ProjectState> Projects { get; set; } = null!;
	public DbSet<ConversionAdvisoryBoardDecisionState> ConversionAdvisoryBoardDecisions { get; set; } = null!;

	public override int SaveChanges()
	{
		SetModifiedAndCreatedDates();
		return base.SaveChanges();
	}

	public EntityEntry<T> ReplaceTracked<T>(T baseEntity) where T : BaseEntity
	{
		var entity = ChangeTracker
			.Entries<T>()
			.SingleOrDefault(s => s.Entity.Id == baseEntity.Id);

		if (entity is null)
		{
			throw new ApplicationException($"An entity matching Id: {baseEntity.Id} is not being tracked");
		}

		var childCollections = entity.Collections
			.Select(collection => collection.CurrentValue);

		foreach (var children in childCollections)
		{
			if (children is null)
			{
				continue;
			}

			foreach (object? child in children)
			{
				Remove(child);
			}
		}

		entity.State = EntityState.Detached;

		return Update(baseEntity);
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
