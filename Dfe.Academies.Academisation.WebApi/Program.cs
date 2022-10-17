using System.Reflection;
using System.Text.Json;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Domain;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Commands.Application.School;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.Application.School;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.CommandValidations;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Academisation.WebApi.AutoMapper;
using Dfe.Academies.Academisation.WebApi.Filters;
using Dfe.Academies.Academisation.WebApi.Middleware;
using Dfe.Academies.Academisation.WebApi.Options;
using Dfe.Academies.Academisation.WebApi.Swagger;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers(x => {
		x.Filters.Add(typeof(HttpGlobalExceptionFilter));
	})
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
		options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
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
//builder.Services.AddScoped<ISetJoinTrustDetailsCommandHandler, JoinTrustCommandHandler>();
builder.Services.AddScoped<IApplicationCreateCommand, ApplicationCreateCommand>();
builder.Services.AddScoped<IApplicationCreateDataCommand, ApplicationCreateDataCommand>();
builder.Services.AddScoped<IApplicationFactory, ApplicationFactory>();
builder.Services.AddScoped<IApplicationSubmitCommand, ApplicationSubmitCommand>();
builder.Services.AddScoped<IApplicationUpdateDataCommand, ApplicationUpdateDataCommand>();
builder.Services.AddScoped<IApplicationUpdateCommand, ApplicationUpdateCommand>();
builder.Services.AddScoped<IApplicationSubmissionService, ApplicationSubmissionService>();
builder.Services.AddScoped<ICreateLoanCommandHandler,CreateLoanCommandHandler>();
builder.Services.AddScoped<ICreateLeaseCommandHandler,CreateLeaseCommandHandler>();
builder.Services.AddScoped<IUpdateLoanCommandHandler,UpdateLoanCommandHandler>();
builder.Services.AddScoped<IUpdateLeaseCommandHandler, UpdateLeaseCommandHandler>();
builder.Services.AddScoped<IDeleteLoanCommandHandler, DeleteLoanCommandHandler>();
builder.Services.AddScoped<IDeleteLeaseCommandHandler, DeleteLeaseCommandHandler>();

//Repositories
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

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

//Validators
builder.Services.AddScoped<IRequest<bool>, CreateLeaseCommand>();
builder.Services.AddScoped<IRequest<bool>, UpdateLeaseCommand>();
builder.Services.AddScoped<IRequest<bool>, CreateLoanCommand>();
builder.Services.AddScoped<IRequest<bool>, UpdateLoanCommand>();
builder.Services.AddScoped<AbstractValidator<CreateLeaseCommand>, CreateLeaseCommandValidator>();
builder.Services.AddScoped<AbstractValidator<UpdateLeaseCommand>, UpdateLeaseCommandValidator>();
builder.Services.AddScoped<AbstractValidator<CreateLoanCommand>, CreateLoanCommandValidator>();
builder.Services.AddScoped<AbstractValidator<UpdateLoanCommand>, UpdateLoanCommandValidator>();

builder.Services.AddScoped(typeof(IValidatorFactory<>), typeof(ValidatorFactory<>));


builder.Services.AddDbContext<AcademisationContext>(options => options
	.UseSqlServer(builder.Configuration["AcademiesDatabaseConnectionString"],
		optionsBuilder => { optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", "academisation"); }));

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerOptions>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(JoinTrustCommandHandler))!);

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

namespace Dfe.Academies.Academisation.WebApi
{
	public partial class Program {
		public static string AppName = GetAppName();

		public static string GetAppName()
		{
			var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

			return appName;
		}
	}
}
