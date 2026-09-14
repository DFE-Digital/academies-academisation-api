using System.Net.Http.Headers;
using System.Reflection;
using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Util;

namespace Dfe.Academies.Academisation.SubcutaneousTest
{
	public abstract class ApiIntegrationTestBase : IDisposable
	{
		private readonly Fixture _fixture;
		private readonly SqliteConnection _sqliteConnection;
		private readonly string _sqliteConnectionString;
		private readonly WebApplicationFactory<Program> _webApplicationFactory;
		protected readonly string _apiKey;
		protected readonly HttpClient _httpClient;
		protected AcademisationContext _dbContext;
		protected readonly WireMockServer _mockApiServer;

		public ApiIntegrationTestBase()
		{
			_fixture = new();
			_apiKey = Guid.NewGuid().ToString();
			_mockApiServer = WireMockServer.Start();
			_sqliteConnectionString = $"Data Source={Guid.NewGuid()};Mode=Memory;Cache=Shared";
			_sqliteConnection = new SqliteConnection(_sqliteConnectionString);
			_sqliteConnection.Open();

			_webApplicationFactory = Build();

			var dbScope = _webApplicationFactory.Services.CreateScope();
			_dbContext = dbScope.ServiceProvider.GetRequiredService<AcademisationContext>();
			_httpClient = BuildHttpClient();
		}

		protected Fixture Fixture => _fixture;
		protected static CancellationToken CancellationToken => CancellationToken.None;

		protected IServiceProvider ServiceProvider
		{
			get => _webApplicationFactory.Services;
		}

		public void Dispose()
		{
			_httpClient.Dispose();
			_dbContext.Dispose();
			_webApplicationFactory.Dispose();
			_mockApiServer.Stop();
			_mockApiServer.Dispose();
			_sqliteConnection.Dispose();
			GC.SuppressFinalize(this);
		}

		private WebApplicationFactory<Program> Build()
		{
			return new WebApplicationFactory<Program>()
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
				   ConfigureInMemoryDatabase(services, _sqliteConnectionString);

				   services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(CreateProjectGroupCommandHandler))!));
			   });
		   });
		}

		private HttpClient BuildHttpClient() 
		{
			var httpClient = _webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
			httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

			return httpClient;
		}

		private void ConfigureInMemoryDatabase(IServiceCollection services, string connectionString)
		{
			services.RemoveAll<AcademisationContext>();
			services.RemoveAll<DbContextOptions<AcademisationContext>>();
			services.RemoveAll<DbContextOptions>();
			services.RemoveAll<IDbContextOptionsConfiguration<AcademisationContext>>();

			services.AddDbContext<AcademisationContext>(options =>
			{
				options.UseSqlite(connectionString);
			});

			using var serviceProvider = services.BuildServiceProvider();
			using var scope = serviceProvider.CreateScope();

			_dbContext = scope.ServiceProvider.GetRequiredService<AcademisationContext>();
			_dbContext.Database.EnsureCreated();
		}


		private void ConfigureAppConfiguration(IWebHostBuilder builder, string apiKey)
		{
			builder.ConfigureAppConfiguration((context, configBuilder) =>
			{
				// Create an in-memory configuration with test values
				var inMemorySettings = new List<KeyValuePair<string, string>>
					{
						new ("AuthenticationConfig:ApiKeys:0", apiKey),
						new ("AcademiesUrl", _mockApiServer.Url!),
						new ("AcademiesApiKey", "f6cbde5b-1252-4439-864c-16956af671d2"),
						new ("RoleIds:ConversionCreation",  "ConversionCreation"),
						new ("RoleIds:TransferCreation",  "TransferCreation"),
						new ("RoleIds:SuperAdmin",  "SuperAdmin")
				};
				configBuilder.AddInMemoryCollection(inMemorySettings!);
			});
		}

		protected void AddGetWithJsonResponse<TResponseBody>(string path, TResponseBody responseBody)
		{
			_mockApiServer
			 .Given(Request.Create()
				.WithPath(path)
				.UsingGet())
			 .RespondWith(Response.Create()
				.WithStatusCode(200)
				.WithHeader("Content-Type", "application/json")
				.WithBody(JsonConvert.SerializeObject(responseBody)));
		}


		protected void AddPatchWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
				  .UsingPatch())
			   .RespondWith(Response.Create()
				  .WithStatusCode(200)
				  .WithHeader("Content-Type", "application/json")
				  .WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		protected void AddApiCallWithBodyDelegate<TResponseBody>(string path, Func<IBodyData, bool> bodyDelegate, TResponseBody responseBody, HttpMethod verb = null!)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .WithBody(bodyDelegate!)
				  .UsingMethod(verb == null ? HttpMethod.Post.ToString() : verb.ToString()))
			   .RespondWith(Response.Create()
				  .WithStatusCode(200)
				  .WithHeader("Content-Type", "application/json")
				  .WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		protected void AddPutWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
				  .UsingPut())
			   .RespondWith(Response.Create()
				  .WithStatusCode(200)
				  .WithHeader("Content-Type", "application/json")
				  .WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		protected void AddPostWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .WithBody(new JsonMatcher(JsonConvert.SerializeObject(requestBody), true))
				  .UsingPost())
			   .RespondWith(Response.Create()
				  .WithStatusCode(200)
				  .WithHeader("Content-Type", "application/json")
				  .WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		protected void AddAnyPostWithJsonRequest<TResponseBody>(string path, TResponseBody responseBody)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .UsingPost())
			   .RespondWith(Response.Create()
				  .WithStatusCode(200)
				  .WithHeader("Content-Type", "application/json")
				  .WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		protected void AddErrorResponse(string path, string method)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .UsingMethod(method))
			   .RespondWith(Response.Create()
				  .WithStatusCode(500));
		}

		protected void Reset()
		{
			_mockApiServer.Reset();
		}
	}
}
