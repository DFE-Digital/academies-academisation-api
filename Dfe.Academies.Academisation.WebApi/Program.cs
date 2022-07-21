using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.Service;
using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.Configure<HelloWorldOptions>(builder.Configuration.GetSection(HelloWorldOptions.Name));

builder.Services.AddScoped<IApplicationCreateCommand, ApplicationCreateCommand>();
builder.Services.AddScoped<IApplicationCreateDataCommand, ApplicationCreateDataCommand>();
builder.Services.AddScoped<IConversionApplicationFactory, ConversionApplicationFactory>();
builder.Services.AddScoped<IApplicationGetQuery, ApplicationGetQuery>();
builder.Services.AddScoped<IApplicationGetDataQuery, ApplicationGetDataQuery>();

builder.Services.AddDbContext<AcademisationContext>(options => options
	.UseSqlServer(builder.Configuration["AcademiesDatabaseConnectionString"], optionsBuilder =>
	{
		optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", "academisation");
	}));

var app = builder.Build();

app.UseSwagger()
	.UseSwaggerUI()
	.UseHttpsRedirection()
	.UseAuthorization();

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();
