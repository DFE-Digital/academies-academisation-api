using Dfe.Academies.Academisation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.Academies.Academisation.Seed
{
	public class DatabaseConfig
	{
		public string? ConnectionString { get; set; }
		public static AcademisationContext? InitialiseDbContext()
		{
			var services = new ServiceCollection();
			// Use builder to populate user secrets
			var builder = new ConfigurationBuilder()
				.AddUserSecrets<DatabaseConfig>()
				.AddEnvironmentVariables();
			var configurationRoot = builder.Build();

			// Set and serve DbContext
			services.AddDbContext<AcademisationContext>(options =>
				options.UseSqlServer(
					connectionString: configurationRoot.GetSection("DatabaseConfig").Get<DatabaseConfig>()!.ConnectionString!));
			var serviceProvider = services.BuildServiceProvider();
			var academisationContext = serviceProvider.GetService<AcademisationContext>();
			return academisationContext;
		}
	}
}
