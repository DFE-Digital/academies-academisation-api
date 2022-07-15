using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data;

public class AcademisationContext : DbContext
{
	public DbSet<ConversionApplicationState> ConversionApplications { get; set; }
	public DbSet<ContributorState> Contributors { get; set; }
	

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}
}
