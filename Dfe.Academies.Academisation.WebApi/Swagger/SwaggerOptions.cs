using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dfe.Academies.Academisation.WebApi.Swagger;

public class SwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
	public void Configure(SwaggerGenOptions options)
	{
		options.AddSecurityDefinition("ApiKey", new()
		{
			Type = SecuritySchemeType.ApiKey,
			In = ParameterLocation.Header,
			Name = "x-api-key",
			Description = "Authorisation API Key",
		});

		options.OperationFilter<AuthenticationHeaderOperationFilter>();
	}
}
