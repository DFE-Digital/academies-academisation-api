using Dfe.Academies.Academisation.Domain.Academies;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfe.Academies.Academisation.Data.Academies;

public class AcademiesContext : DbContext, IUnitOfWork
{
	const string DEFAULT_SCHEMA = "mstr";
	public AcademiesContext(DbContextOptions<AcademiesContext> options) : base(options)
	{

	}
	
	public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		await base.SaveChangesAsync(cancellationToken);
		return true;
	}
	
	public DbSet<Trust> Trusts { get; set; }
	public DbSet<School> Schools { get; set; }
	public DbSet<LocalAuthority> LocalAuthorities { get; set; }
	public DbSet<Region> Regions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Trust>(ConfigureTrust);
		modelBuilder.Entity<School>(ConfigureSchool);
		modelBuilder.Entity<LocalAuthority>(ConfigureLocalAuthority);
		modelBuilder.Entity<Region>(ConfigureRegion);
		
		base.OnModelCreating(modelBuilder);
	}
	
	void ConfigureTrust(EntityTypeBuilder<Trust> trustConfiguration)
	{
		trustConfiguration.ToTable("Trust", DEFAULT_SCHEMA);

		trustConfiguration.HasKey(a => a.Id);
		trustConfiguration
			.Property(i => i.Id)
			.HasColumnName("SK");
		trustConfiguration.Property(i => i.GroupUId)
			.HasColumnName("Group UID");
		trustConfiguration.Property(i => i.GroupId)
			.HasColumnName("Group ID");
		trustConfiguration.Property(i => i.ReferenceId)
			.HasColumnName("RID");
		trustConfiguration.Property(i => i.UKPRN)
			.HasColumnName("UKPRN");
		trustConfiguration.Property(i => i.CompaniesHouseNumber)
			.HasColumnName("Companies House Number");

		trustConfiguration.Property(i => i.RegionId)
			.HasColumnName("FK_Region");

		trustConfiguration
			.HasOne(x => x.Region)
			.WithOne()
			.HasForeignKey<Trust>(x => x.RegionId);

	}

	void ConfigureRegion(EntityTypeBuilder<Region> regionConfiguration)
	{
		regionConfiguration.ToTable("Ref_Region", DEFAULT_SCHEMA);

		regionConfiguration.HasKey(a => a.Id);
		regionConfiguration
			.Property(i => i.Id)
			.HasColumnName("SK");
		
		regionConfiguration.Property(i => i.Name)
			.HasColumnName("Name");
	}

	void ConfigureSchool(EntityTypeBuilder<School> schoolConfiguration)
	{
		schoolConfiguration.ToTable("EducationEstablishment", DEFAULT_SCHEMA);

		schoolConfiguration.HasKey(i => i.Id);
		schoolConfiguration.Property(i => i.Id)
			.HasColumnName("SK");

		schoolConfiguration.Property(i => i.Name)
			.HasColumnName("EstablishmentName");
		
		schoolConfiguration.Property(i => i.LocalAuthorityId)
			.HasColumnName("FK_LocalAuthority");
		
		schoolConfiguration.HasOne<LocalAuthority>(s => s.LocalAuthority)
			.WithMany(x => x.Schools)
			.HasForeignKey(s => s.LocalAuthorityId);
	}

	void ConfigureLocalAuthority(EntityTypeBuilder<LocalAuthority> localAuthorityConfiguration)
	{
		localAuthorityConfiguration.ToTable("Ref_LocalAuthority", DEFAULT_SCHEMA);

		localAuthorityConfiguration.HasKey(i => i.Id);
		localAuthorityConfiguration.Property(i => i.Id)
			.HasColumnName("SK");

		localAuthorityConfiguration.Property(i => i.Name)
			.HasColumnName("Name");
		localAuthorityConfiguration.Property(i => i.Code)
			.HasColumnName("Code");
	}

}
