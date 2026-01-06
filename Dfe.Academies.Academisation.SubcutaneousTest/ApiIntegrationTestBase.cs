using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Encodings.Web;
using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.WebApi;
using Dfe.Academies.Academisation.WebApi.Options;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
		private static int _currentPort = 5080;
		private static readonly object Sync = new();
		private readonly Fixture _fixture;
		protected readonly string _apiKey;
		protected HttpClient _httpClient;
		protected AcademisationContext _dbContext;
		protected readonly WireMockServer _mockApiServer;
		public ApiIntegrationTestBase()
		{
			int port = AllocateNext();
			_fixture = new();
			_apiKey = Guid.NewGuid().ToString();
			_mockApiServer = WireMockServer.Start(port);
			_httpClient =Build();
			_dbContext = GetDbContext();
		}

		private WebApplicationFactory<Program> WebAppFactory { get; set; } = new();

		protected Fixture Fixture => _fixture;

		protected AcademisationContext GetDbContext()
		{
			var dbContext = ServiceProvider.GetRequiredService<AcademisationContext>();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			return dbContext;
		}
		protected static CancellationToken CancellationToken => CancellationToken.None;

		protected IServiceProvider ServiceProvider
		{
			get
			{
				return WebAppFactory.Services;
			}
		}
		void IDisposable.Dispose() 
		{
			_httpClient?.Dispose();
			_mockApiServer?.Stop();
			_mockApiServer?.Dispose();
		} 

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
			var establishmentDto = new EstablishmentDto
			{
				Ukprn = "OutgoingAcademyUkprn",
				Name = "School Name",
				EstablishmentType = new NameAndCodeDto { Name = "EstablishmentType" },
				LocalAuthorityName = "Manchester",
				Gor = new NameAndCodeDto { Name = "Gor" }
			};

			var query = ToQueryString((new List<string> { establishmentDto.Ukprn }).Select(ukprn =>
			{
				return new KeyValuePair<string, string>("Ukprn", ukprn);
			})
			.ToList());

			builder.ConfigureServices((context, services) =>
			{
				// Bind the configuration section to the AuthenticationConfig class
				var configuration = context.Configuration;
				services.Configure<AuthenticationConfig>(configuration.GetSection("AuthenticationConfig"));

			});
		}

		private static string ToQueryString(IList<KeyValuePair<string, string>> parameters, bool prefix = true,
		   bool keepEmpty = true)
		{
			IList<string> parameterPairs = parameters
			   .Where(x => keepEmpty || string.IsNullOrWhiteSpace(x.Value) is false)
			   .Select(x => $"{Encode(x.Key)}={Encode(x.Value)}")
			   .ToList();

			var prefixContent = prefix ? "?" : string.Empty;

			return parameterPairs.Count > 0
			   ? $"{prefixContent}{string.Join("&", parameterPairs)}"
			   : string.Empty;

			string Encode(string x)
			{
				return string.IsNullOrWhiteSpace(x) ? string.Empty : UrlEncoder.Default.Encode(x);
			}
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

		private static int AllocateNext()
		{
			lock (Sync)
			{
				int next = _currentPort;
				_currentPort++;
				return next;
			}
		}
	}
}
