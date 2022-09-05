using System.Text.Json.Serialization;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.Middleware;
using Dfe.Academies.Academisation.WebApi.Options;
using Dfe.Academies.Academisation.WebApi.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
builder.Services.AddScoped<IConversionAdvisoryBoardDecisionGetQuery, ConversionAdvisoryBoardDecisionGetQuery>();
builder.Services.AddScoped<IAdvisoryBoardDecisionGetDataByProjectIdQuery, AdvisoryBoardDecisionGetDataByProjectIdQuery>();
builder.Services.AddScoped<IAdvisoryBoardDecisionGetDataByDecisionIdQuery, AdvisoryBoardDecisionGetDataByDecisionIdQuery>();
builder.Services.AddScoped<ILegacyProjectGetQuery, LegacyProjectGetQuery>();
builder.Services.AddScoped<IProjectGetDataQuery, ProjectGetDataQuery>();

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

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();

public partial class Program {}
