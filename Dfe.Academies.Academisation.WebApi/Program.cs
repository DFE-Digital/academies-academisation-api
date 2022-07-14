using Dfe.Academies.Academisation.WebApi.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.Configure<HelloWorldOptions>(builder.Configuration.GetSection(HelloWorldOptions.Name));

var app = builder.Build();

app.UseSwagger()
	.UseSwaggerUI()
	.UseHttpsRedirection()
	.UseAuthorization();

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();