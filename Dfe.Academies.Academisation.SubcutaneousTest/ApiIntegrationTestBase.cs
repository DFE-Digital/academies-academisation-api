using System.Net.Http.Headers;
using System.Reflection;
using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.WebApi;
using Dfe.Academies.Academisation.WebApi.Options;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.SubcutaneousTest
{
	public class ApiIntegrationTestBase
	{
		private readonly Fixture _fixture;
		protected readonly string _apiKey; 
		private HttpClient _httpClient;
		private IMediator _mediator;
		private AcademisationContext _dbContext;
		public ApiIntegrationTestBase() {
			_fixture = new();
			_apiKey = Guid.NewGuid().ToString();
			_httpClient = Build();
			_mediator = ServiceProvider.GetRequiredService<IMediator>();
			_dbContext = ServiceProvider.GetRequiredService<AcademisationContext>();
		}

		protected HttpClient CreateClient() { return _httpClient; }

		private WebApplicationFactory<Program> WebAppFactory { get; set; } = new();

		protected Fixture Fixture => _fixture;


		protected AcademisationContext GetDBContext()
		{
			_dbContext.Database.EnsureDeleted();
			_dbContext.Database.EnsureCreated();
			return _dbContext;

		}
		protected static CancellationToken CancellationToken => CancellationToken.None;

		protected IServiceProvider ServiceProvider
		{
			get
			{
				return WebAppFactory.Services;
			}
		}

		protected ApiIntegrationTestBase GetHttpClient => this;

		private HttpClient Build()
		{
			WebAppFactory = new WebApplicationFactory<Program>()
		   .WithWebHostBuilder(builder =>
		   {
			   builder.UseEnvironment("local");

			   builder.ConfigureLogging(x =>
			   {
				   x.ClearProviders();
				   x.SetMinimumLevel(LogLevel.Debug);
			   });

			   ConfigureAppConfiguration(builder, _apiKey);

			   builder.ConfigureTestServices(services =>
			   {
				   ConfigureInMemoryDatabase(services);
				   ConfigureServices(builder);

				   services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(CreateProjectGroupCommandHandler))!));
			   });
		   });

			WebAppFactory.Server.PreserveExecutionContext = true;

			return BuildHttpClient();
		}

		private HttpClient BuildHttpClient() 
		{
			var httpClient = WebAppFactory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
			httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

			return httpClient;
		}

		private static void ConfigureServices(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
				// Bind the configuration section to the AuthenticationConfig class
				var configuration = context.Configuration;
				services.Configure<AuthenticationConfig>(configuration.GetSection("AuthenticationConfig"));
			});
		}

		private static void ConfigureInMemoryDatabase(IServiceCollection services)
		{
			// Replace database context with our own Integration database
			var descriptor = services.SingleOrDefault(d =>
				d.ServiceType == typeof(DbContextOptions<AcademisationContext>));
			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			var connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();

			services.AddDbContext<AcademisationContext>(options =>
			{
				options.UseSqlite(connection);
			});
		}
		private static void ConfigureAppConfiguration(IWebHostBuilder builder, string apiKey)
		{
			builder.ConfigureAppConfiguration((context, configBuilder) =>
			{
				// Create an in-memory configuration with test values
				var inMemorySettings = new List<KeyValuePair<string, string>>
					{
						new ("AuthenticationConfig:ApiKeys:0", apiKey)
					};

				configBuilder.AddInMemoryCollection(inMemorySettings!);
			});
		}
	}
}
