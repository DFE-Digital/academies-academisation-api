using System.Reflection.Emit;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext, IUnitOfWork
{
	const string DEFAULT_SCHEMA = "academisation";
	public AcademisationContext(DbContextOptions<AcademisationContext> options) : base(options)
	{
		
	}

	public DbSet<Application> Applications { get; set; } = null!; // done
	public DbSet<Contributor> Contributors { get; set; } = null!; // done
	public DbSet<School> Schools { get; set; } = null!; // done
	public DbSet<LoanState> SchoolLoans { get; set; } = null!;  // done
	public DbSet<LeaseState> SchoolLeases { get; set; } = null!; // done

	public DbSet<JoinTrust> JoinTrusts { get; set; } = null!; // done
	public DbSet<FormTrust> FormTrusts { get; set; } = null!; // done
                                                           
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
			throw new InvalidOperationException($"An entity matching Id: {baseEntity.Id} is not being tracked");
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
				Type childType = child.GetType();

				if (childType == typeof(ApplicationSchoolState))
				{
					var schoolState = (ApplicationSchoolState)child;

					foreach (var loan in schoolState.Loans)
					{
						Remove(loan);
					}

					foreach (var lease in schoolState.Leases)
					{
						Remove(lease);
					}
				}

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

	public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		//This will be used to dispatch domain events in mediator before committing changes with SaveChangesAsync
		/*
		 * await _mediator.DispatchDomainEventsAsync(this);
		 */
		await base.SaveChangesAsync(cancellationToken);
		return true;
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
		modelBuilder.Entity<Application>(ConfigureApplication);
		modelBuilder.Entity<School>(ConfigureSchool);
		modelBuilder.Entity<Contributor>(ConfigureContributor);
		modelBuilder.Entity<FormTrust>(ConfigureFormTrust);
		modelBuilder.Entity<JoinTrust>(ConfigureJoinTrust);
		modelBuilder.Entity<Loan>(ConfigureLoan);
		modelBuilder.Entity<Lease>(ConfigureLease);
		modelBuilder.Entity<TrustKeyPerson>(ConfigureTrustKeyPerson);
		modelBuilder.Entity<TrustKeyPersonRole>(ConfigureTrustKeyPersonRole);

		OnAdvisoryBoardDecisionCreating(modelBuilder);

		OnProjectCreating(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	private static void OnProjectCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ProjectState>()
			.HasMany(x => x.Notes)
			.WithOne()
			.HasForeignKey("ProjectId");

		modelBuilder.Entity<ProjectState>()
			.Property(e => e.GoverningBodyResolution).HasConversion<string>();
		modelBuilder.Entity<ProjectState>()
			.Property(e => e.Consultation).HasConversion<string>();
		modelBuilder.Entity<ProjectState>()
			.Property(e => e.DiocesanConsent).HasConversion<string>();
		modelBuilder.Entity<ProjectState>()
			.Property(e => e.FoundationConsent).HasConversion<string>();
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

	/// <summary>
	/// New mapping for refactoring
	/// </summary>
	/// <param name="applicationConfiguration"></param>

	void ConfigureApplication(EntityTypeBuilder<Application> applicationConfiguration)
	{
		applicationConfiguration.ToTable("ConversionApplication", DEFAULT_SCHEMA);
		applicationConfiguration
		.Property(e => e.ApplicationType)
			.HasConversion<string>();

		applicationConfiguration
		.Property(e => e.ApplicationStatus)
			.HasConversion<string>();

		applicationConfiguration
			.Property(p => p.ApplicationReference)
			.HasComputedColumnSql("'A2B_' + CAST([Id] AS NVARCHAR(255))", stored: true);

		applicationConfiguration.HasKey(a => a.Id);

		var navigation = applicationConfiguration.Metadata.FindNavigation(nameof(Application.Schools));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	void ConfigureSchool(EntityTypeBuilder<School> schoolConfiguration)
	{
		schoolConfiguration.ToTable("ApplicationSchool", DEFAULT_SCHEMA);
		schoolConfiguration.HasKey(a => a.Id);

		var loanNavigation = schoolConfiguration.Metadata.FindNavigation(nameof(School.Loans));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		loanNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		var leaseNavigation = schoolConfiguration.Metadata.FindNavigation(nameof(School.Leases));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		leaseNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		schoolConfiguration.HasOne<Application>()
			.WithMany()
			.IsRequired(false)
			.HasForeignKey("ConversionApplicationId");
	}

	void ConfigureContributor(EntityTypeBuilder<Contributor> contributorConfiguration)
	{
		contributorConfiguration.ToTable("ConversionApplicationContributor", DEFAULT_SCHEMA);
		contributorConfiguration.HasKey(a => a.Id);

		contributorConfiguration.HasOne<Application>()
			.WithMany()
			.IsRequired(false)
			.HasForeignKey("ConversionApplicationId");
	}

	void ConfigureFormTrust(EntityTypeBuilder<FormTrust> formTrustConfiguration)
	{
		formTrustConfiguration.ToTable("ApplicationFormTrust", DEFAULT_SCHEMA);
		formTrustConfiguration.HasKey(a => a.Id);

		var navigation = formTrustConfiguration.Metadata.FindNavigation(nameof(FormTrust.KeyPeople));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
	}

	void ConfigureJoinTrust(EntityTypeBuilder<JoinTrust> joinTrustConfiguration)
	{
		joinTrustConfiguration.ToTable("ApplicationJoinTrust", DEFAULT_SCHEMA);
		joinTrustConfiguration.HasKey(a => a.Id);
	}

	void ConfigureLoan(EntityTypeBuilder<Loan> loanConfiguration)
	{
		loanConfiguration.ToTable("ApplicationSchoolLoan", DEFAULT_SCHEMA);
		loanConfiguration.HasKey(a => a.Id);

		loanConfiguration.HasOne<School>()
			.WithMany()
			.IsRequired(false)
			.HasForeignKey("ApplicationSchoolId");
	}

	void ConfigureLease(EntityTypeBuilder<Lease> leaseConfiguration)
	{
		leaseConfiguration.ToTable("ApplicationSchoolLease", DEFAULT_SCHEMA);
		leaseConfiguration.HasKey(a => a.Id);

		leaseConfiguration.HasOne<School>()
			.WithMany()
			.IsRequired(false)
			.HasForeignKey("ApplicationSchoolId");
	}

	void ConfigureTrustKeyPerson(EntityTypeBuilder<TrustKeyPerson> trustKeyPersonConfiguration)
	{
		trustKeyPersonConfiguration.ToTable("ApplicationFormTrustKeyPerson", DEFAULT_SCHEMA);
		trustKeyPersonConfiguration.HasKey(a => a.Id);

		var navigation = trustKeyPersonConfiguration.Metadata.FindNavigation(nameof(TrustKeyPerson.Roles));
		// DDD Patterns comment:
		//Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
		navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		trustKeyPersonConfiguration.HasOne<FormTrust>()
			.WithMany()
			.IsRequired(false)
			.HasForeignKey("ApplicationFormTrustId");
	}

	void ConfigureTrustKeyPersonRole(EntityTypeBuilder<TrustKeyPersonRole> trustKeyPersonRoleConfiguration)
	{
		trustKeyPersonRoleConfiguration.ToTable("ApplicationFormTrustKeyPersonRole", DEFAULT_SCHEMA);
		trustKeyPersonRoleConfiguration.HasKey(a => a.Id);

		trustKeyPersonRoleConfiguration.HasOne<TrustKeyPerson>()
			.WithMany()
			.IsRequired(false)
			.HasForeignKey("ApplicationFormTrustKeyPersonRoleId");
	}
}
