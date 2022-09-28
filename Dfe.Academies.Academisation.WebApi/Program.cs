using System.Text.Json;
using System.Text.Json.Serialization;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Commands.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.Project;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Filters;
using Dfe.Academies.Academisation.WebApi.Middleware;
using Dfe.Academies.Academisation.WebApi.Options;
using Dfe.Academies.Academisation.WebApi.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers(x => {
		x.Filters.Add(typeof(HttpGlobalExceptionFilter));
	})
	.AddNewtonsoftJson()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

// logging
builder.Host.ConfigureLogging((context, logging) =>
{
	logging.AddConfiguration(context.Configuration.GetSection("Logging"));

	logging.ClearProviders();

	//logging.AddSimpleConsole(options =>
	//{
	//	options.IncludeScopes = true;
	//	options.SingleLine = true;
	//	options.TimestampFormat = "hh:mm:ss ";
	//});

	logging.AddJsonConsole(options =>
	{
		options.IncludeScopes = true;
		options.TimestampFormat = "hh:mm:ss ";
		options.JsonWriterOptions = new JsonWriterOptions
		{
			Indented = true,
		};
	});

	logging.AddSentry();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
	config.SwaggerDoc("v1", new()
	{
		Title = "Academisation API",
		Version = "v1"
	});
});

builder.Services.AddHealthChecks();

builder.Services.AddOptions<AuthenticationConfig>();
var apiKeysConfiguration = builder.Configuration.GetSection("AuthenticationConfig");
builder.Services.Configure<AuthenticationConfig>(apiKeysConfiguration);

// Commands
builder.Services.AddScoped<IApplicationCreateCommand, ApplicationCreateCommand>();
builder.Services.AddScoped<IApplicationCreateDataCommand, ApplicationCreateDataCommand>();
builder.Services.AddScoped<IApplicationFactory, ApplicationFactory>();
builder.Services.AddScoped<IApplicationSubmitCommand, ApplicationSubmitCommand>();
builder.Services.AddScoped<IApplicationUpdateDataCommand, ApplicationUpdateDataCommand>();
builder.Services.AddScoped<IApplicationUpdateCommand, ApplicationUpdateCommand>();
builder.Services.AddScoped<IApplicationSubmissionService, ApplicationSubmissionService>();

builder.Services.AddScoped<IProjectCreateDataCommand, ProjectCreateDataCommand>();
builder.Services.AddScoped<IProjectUpdateDataCommand, ProjectUpdateDataCommand>();
builder.Services.AddScoped<ILegacyProjectUpdateCommand, LegacyProjectUpdateCommand>();

builder.Services.AddScoped<IConversionAdvisoryBoardDecisionFactory, ConversionAdvisoryBoardDecisionFactory>();
builder.Services.AddScoped<IAdvisoryBoardDecisionCreateCommand, AdvisoryBoardDecisionCreateCommand>();
builder.Services.AddScoped<IAdvisoryBoardDecisionCreateDataCommand, AdvisoryBoardDecisionCreateDataCommand>();
builder.Services.AddScoped<IAdvisoryBoardDecisionUpdateCommand, AdvisoryBoardDecisionUpdateCommand>();
builder.Services.AddScoped<IAdvisoryBoardDecisionUpdateDataCommand, AdvisoryBoardDecisionUpdateDataCommand>();

// Queries
builder.Services.AddScoped<IApplicationGetQuery, ApplicationGetQuery>();
builder.Services.AddScoped<IApplicationGetDataQuery, ApplicationGetDataQuery>();
builder.Services.AddScoped<IApplicationsListByUserDataQuery, ApplicationsListByUserDataQuery>();
builder.Services.AddScoped<IApplicationListByUserQuery, ApplicationListByUserQuery>();
builder.Services.AddScoped<IProjectGetDataQuery, ProjectGetDataQuery>();
builder.Services.AddScoped<IConversionAdvisoryBoardDecisionGetQuery, ConversionAdvisoryBoardDecisionGetQuery>();
builder.Services.AddScoped<IAdvisoryBoardDecisionGetDataByProjectIdQuery, AdvisoryBoardDecisionGetDataByProjectIdQuery>();
builder.Services.AddScoped<IAdvisoryBoardDecisionGetDataByDecisionIdQuery, AdvisoryBoardDecisionGetDataByDecisionIdQuery>();
builder.Services.AddScoped<ILegacyProjectGetQuery, LegacyProjectGetQuery>();
builder.Services.AddScoped<ILegacyProjectListGetQuery, LegacyProjectListGetQuery>();
builder.Services.AddScoped<IProjectListGetDataQuery, ProjectListGetDataQuery>();

// Factories
builder.Services.AddScoped<IProjectFactory, ProjectFactory>();

builder.Services.AddDbContext<AcademisationContext>(options => options
	.UseSqlServer(builder.Configuration["AcademiesDatabaseConnectionString"],
		optionsBuilder => { optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", "academisation"); }));

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerOptions>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
	app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
}
app.UseMiddleware<AddCorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();

public partial class Program {
	public static string AppName = GetAppName();

	public static string GetAppName()
	{
		//var theNamespace = typeof(Program).Namespace;
		//return theNamespace[(theNamespace.LastIndexOf('.', theNamespace.LastIndexOf('.') - 1) + 1)..];

		return "Dfe.Academies.Academisation.WebApi";
	}
}
