using System.Reflection;
using System.Text;
using System.Text.Json;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Domain;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.Behaviours;
using Dfe.Academies.Academisation.Service.Commands.Application.School;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Dfe.Academies.Academisation.Service.CommandValidations;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using Dfe.Academies.Academisation.Service.Mappers.OpeningDateHistoryMapper;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using Dfe.Academies.Academisation.WebApi.Filters;
using Dfe.Academies.Academisation.WebApi.Middleware;
using Dfe.Academies.Academisation.WebApi.Options;
using Dfe.Academies.Academisation.WebApi.Services;
using Dfe.Academies.Academisation.WebApi.Swagger;
using Dfe.Academisation.CorrelationIdMiddleware;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetEscapades.AspNetCore.SecurityHeaders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers(x =>
	{
		x.Filters.Add(typeof(HttpGlobalExceptionFilter));
	})
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
		options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
		options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
	});

// logging
builder.Host.ConfigureLogging((context, logging) =>
{
	logging.AddConfiguration(context.Configuration.GetSection("Logging"));

	logging.ClearProviders();

	logging.AddJsonConsole(options =>
	{
		options.IncludeScopes = true;
		options.TimestampFormat = "hh:mm:ss ";
		options.JsonWriterOptions = new JsonWriterOptions
		{
			Indented = true,
		};
	});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
	config.SwaggerDoc("v1", new()
	{
		Title = "Academisation API",
		Version = "v1"
	});

	string descriptions = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	string descriptionsPath = Path.Combine(AppContext.BaseDirectory, descriptions);
	if (File.Exists(descriptionsPath)) config.IncludeXmlComments(descriptionsPath);
});

builder.Services.AddHealthChecks();

builder.Services.AddOptions<AuthenticationConfig>();
var apiKeysConfiguration = builder.Configuration.GetSection("AuthenticationConfig");
builder.Services.Configure<AuthenticationConfig>(apiKeysConfiguration);

// Commands
builder.Services.AddScoped<IEnrichProjectCommand, EnrichProjectCommand>();
builder.Services.AddScoped<IEnrichTransferProjectCommand, EnrichTransferProjectCommand>();
builder.Services.AddScoped<IProjectUpdateDataCommand, ProjectUpdateDataCommand>();

// Repositories
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<ITransferProjectRepository, TransferProjectRepository>();
builder.Services.AddScoped<IProjectGroupRepository, ProjectGroupRepository>();
builder.Services.AddScoped<IConversionProjectRepository, ConversionProjectRepository>();
builder.Services.AddScoped<IFormAMatProjectRepository, FormAMatProjectRepository>();
builder.Services.AddScoped<IAdvisoryBoardDecisionRepository, AdvisoryBoardDecisionRepository>();
builder.Services.AddScoped<IOpeningDateHistoryRepository, OpeningDateHistoryRepository>();



// Queries and services
builder.Services.AddScoped<IApplicationSubmissionService, ApplicationSubmissionService>();
builder.Services.AddScoped<IApplicationQueryService, ApplicationQueryService>();
builder.Services.AddScoped<IConversionProjectQueryService, ConversionProjectQueryService>();
builder.Services.AddScoped<IConversionProjectExportService, ConversionProjectExportService>();
builder.Services.AddScoped<IAdvisoryBoardDecisionQueryService, AdvisoryBoardDecisionGetQueryService>();
builder.Services.AddScoped<IAcademiesQueryService, AcademiesQueryService>();
builder.Services.AddScoped<ITrustQueryService, TrustQueryService>();
builder.Services.AddScoped<ITransferProjectQueryService, TransferProjectQueryService>();
builder.Services.AddScoped<ITransferProjectExportService, TransferProjectExportService>();
builder.Services.AddScoped<IProjectGroupQueryService, ProjectGroupQueryService>();

// utils
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICorrelationContext, CorrelationContext>();
builder.Services.AddAutoMapper(typeof(OpeningDateHistoryMappingProfile).Assembly);

// Factories
builder.Services.AddScoped<IProjectFactory, ProjectFactory>();
builder.Services.AddScoped<IConversionAdvisoryBoardDecisionFactory, ConversionAdvisoryBoardDecisionFactory>();
builder.Services.AddScoped<IAcademiesApiClientFactory, AcademiesApiClientFactoryFactory>();
builder.Services.AddScoped<IApplicationFactory, ApplicationFactory>();

//Validators
builder.Services.AddSingleton<ICypressKeyValidator, CypressKeyValidator>();

builder.Services.AddDbContext<AcademisationContext>(options =>
	{
		options.UseSqlServer(builder.Configuration["AcademiesDatabaseConnectionString"],
			optionsBuilder => { optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", "academisation"); });
#if DEBUG
		options.LogTo(Console.WriteLine, LogLevel.Information);
#endif
	}
);

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerOptions>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddHttpContextAccessor();

builder.Services
	.AddOptions<SwaggerUIOptions>()
	.Configure<IHttpContextAccessor>((swaggerUiOptions, httpContextAccessor) =>
	{
		// 2. Take a reference of the original Stream factory which reads from Swashbuckle's embedded resources
		var originalIndexStreamFactory = swaggerUiOptions.IndexStream;

		// 3. Override the Stream factory
		swaggerUiOptions.IndexStream = () =>
		{
			// 4. Read the original index.html file
			using var originalStream = originalIndexStreamFactory();
			using var originalStreamReader = new StreamReader(originalStream);
			var originalIndexHtmlContents = originalStreamReader.ReadToEnd();

			// 5. Get the request-specific nonce generated by NetEscapades.AspNetCore.SecurityHeaders
			var requestSpecificNonce = httpContextAccessor.HttpContext.GetNonce();

			// 6. Replace inline `<script>` and `<style>` tags by adding a `nonce` attribute to them
			var nonceEnabledIndexHtmlContents = originalIndexHtmlContents
				.Replace("<script>", $"<script nonce=\"{requestSpecificNonce}\">", StringComparison.OrdinalIgnoreCase)
				.Replace("<style>", $"<style nonce=\"{requestSpecificNonce}\">", StringComparison.OrdinalIgnoreCase);

			// 7. Return a new Stream that contains our modified contents
			return new MemoryStream(Encoding.UTF8.GetBytes(nonceEnabledIndexHtmlContents));
		};
	});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateLoanCommandHandler).GetTypeInfo().Assembly));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

builder.Services.AddScoped(typeof(IValidator<UpdateLoanCommand>), typeof(UpdateLoanCommandValidator));
builder.Services.AddScoped(typeof(IValidator<CreateLoanCommand>), typeof(CreateLoanCommandValidator));
builder.Services.AddScoped(typeof(IValidator<UpdateLeaseCommand>), typeof(UpdateLeaseCommandValidator));
builder.Services.AddScoped(typeof(IValidator<CreateLeaseCommand>), typeof(CreateLeaseCommandValidator));
builder.Services.AddScoped(typeof(IValidator<CreateTransferProjectCommand>), typeof(CreateTransferProjectCommandValidator));
builder.Services.AddScoped(typeof(IValidator<CreateProjectGroupCommand>), typeof(CreateProjectGroupCommandValidator));
builder.Services.AddScoped(typeof(IValidator<SetProjectGroupCommand>), typeof(SetProjectGroupCommandValidator));
builder.Services.AddScoped(typeof(IValidator<SetProjectGroupAssignUserCommand>), typeof(SetProjectGroupAssignUserCommandValidator));

builder.Services.AddHostedService<EnrichProjectService>();
builder.Services.AddHostedService<CreateFormAMatProjectsService>();
builder.Services.AddHostedService<SetFormAMatProjectReferenceNumbersService>();

builder.Services.AddHttpClient("AcademiesApi", (sp, client) =>
{
	var configuration = sp.GetRequiredService<IConfiguration>();
	var url = configuration.GetValue<string?>("AcademiesUrl");
	if (!string.IsNullOrEmpty(url))
	{
		client.BaseAddress = new Uri(url);
		client.DefaultRequestHeaders.Add("ApiKey", configuration.GetValue<string>("AcademiesApiKey"));
	}
	else
	{
		sp.GetRequiredService<ILogger<Program>>().LogError("Academies API http client not configured.");
	}
});


builder.Services.AddApplicationInsightsTelemetry();
var aiOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();

// Disables adaptive sampling.
aiOptions.EnableAdaptiveSampling = false;

// Disables QuickPulse (Live Metrics stream).
aiOptions.EnableQuickPulseMetricStream = false;
aiOptions.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
builder.Services.AddApplicationInsightsTelemetry(aiOptions);

var app = builder.Build();

app.UseSecurityHeaders(options =>
{
	options.AddFrameOptionsDeny()
		.AddXssProtectionDisabled()
		.AddContentTypeOptionsNoSniff()
		.RemoveServerHeader()
		.AddContentSecurityPolicy(builder =>
		{
			builder.AddDefaultSrc().None();

			// Only allow loading resources from this app by default
			builder.AddDefaultSrc().Self();

			builder.AddStyleSrc()
			   .Self()
			   // Allow nonce-enabled <style> tags
			   .WithNonce();

			builder.AddScriptSrc()
			   .Self()
			   // Allow nonce-enabled <script> tags
			   .WithNonce();
		})
		.AddPermissionsPolicy(builder =>
		{
			builder.AddAccelerometer().None();
			builder.AddAutoplay().None();
			builder.AddCamera().None();
			builder.AddEncryptedMedia().None();
			builder.AddFullscreen().None();
			builder.AddGeolocation().None();
			builder.AddGyroscope().None();
			builder.AddMagnetometer().None();
			builder.AddMicrophone().None();
			builder.AddMidi().None();
			builder.AddPayment().None();
			builder.AddPictureInPicture().None();
			builder.AddSyncXHR().None();
			builder.AddUsb().None();
		});
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
app.UseMiddleware<CypressApiKeyMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();

namespace Dfe.Academies.Academisation.WebApi
{
	public partial class Program
	{
		public static string AppName = GetAppName();

		public static string GetAppName()
		{
			var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

			return appName;
		}
	}
}
