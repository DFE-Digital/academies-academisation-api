using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.Service;
using Dfe.Academies.Academisation.WebApi.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.Configure<HelloWorldOptions>(builder.Configuration.GetSection(HelloWorldOptions.Name));

builder.Services.AddScoped<IApplicationCreateCommand, ApplicationCreateCommand>();
builder.Services.AddScoped<IConversionApplicationFactory, ConversionApplicationFactory>();

var app = builder.Build();

app.UseSwagger()
	.UseSwaggerUI()
	.UseHttpsRedirection()
	.UseAuthorization();

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();