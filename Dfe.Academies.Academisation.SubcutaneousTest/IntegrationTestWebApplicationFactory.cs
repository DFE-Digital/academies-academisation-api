using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.WebApi;
using Dfe.Academies.Academisation.WebApi.Options;
using Dfe.Academies.Contracts.V4.Establishments;
using MediatR;
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
using Xunit.Abstractions;

namespace Dfe.Academies.Academisation.SubcutaneousTest
{
	public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program> 
	{
		private static int _currentPort = 5080;
		private static readonly object Sync = new();
		private readonly WireMockServer _mockApiServer; 
		protected readonly string _apiKey;
		public IntegrationTestWebApplicationFactory()
		{
			_apiKey = Guid.NewGuid().ToString(); 
			int port = AllocateNext();
			_mockApiServer = WireMockServer.Start(port);
			_mockApiServer.LogEntriesChanged += EntriesChanged!;
		} 

		public HttpClient? HttpClient { get; set; }
		protected ITestOutputHelper? DebugOutput { get; set; }

		public AcademisationContext GetDBContext()
		{
			var dbContext = Services.GetService<AcademisationContext>();
			dbContext!.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			return dbContext;
		}
		protected override void ConfigureWebHost(IWebHostBuilder builder)
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
			Server.PreserveExecutionContext = true;
			CreateClient();
		}

		private void CreateHttpClient()
		{
			HttpClient = CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
			HttpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
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
		public void AddGetWithJsonResponse<TResponseBody>(string path, TResponseBody responseBody)
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


		public void AddPatchWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
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

		public void AddApiCallWithBodyDelegate<TResponseBody>(string path, Func<IBodyData, bool> bodyDelegate, TResponseBody responseBody, HttpMethod verb = null)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .WithBody(bodyDelegate)
				  .UsingMethod(verb == null ? HttpMethod.Post.ToString() : verb.ToString()))
			   .RespondWith(Response.Create()
				  .WithStatusCode(200)
				  .WithHeader("Content-Type", "application/json")
				  .WithBody(JsonConvert.SerializeObject(responseBody)));
		}

		public void AddPutWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
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

		public void AddPostWithJsonRequest<TRequestBody, TResponseBody>(string path, TRequestBody requestBody, TResponseBody responseBody)
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

		public void AddAnyPostWithJsonRequest<TResponseBody>(string path, TResponseBody responseBody)
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

		public void AddErrorResponse(string path, string method)
		{
			_mockApiServer
			   .Given(Request.Create()
				  .WithPath(path)
				  .UsingMethod(method))
			   .RespondWith(Response.Create()
				  .WithStatusCode(500));
		}

		public void Reset()
		{
			_mockApiServer.Reset();
		}
		private void EntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			DebugOutput?.WriteLine($"API Server change: {JsonConvert.SerializeObject(e)}");
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

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				_mockApiServer.Stop();
			}
		}
	}
}
