using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.WebApi.Middleware;

public class ApiKeyAuthenticationMiddleware
{
	private const string ApiKeyHeader = "x-api-key";
	private const string ApiKeyMissingResponse = "API key was not provided";
	private const string UnauthorisedResponse = "Unauthorised Client";
	private readonly RequestDelegate _next;
	private readonly ICollection<string> _apiKeys;

	public ApiKeyAuthenticationMiddleware(RequestDelegate next, IOptions<AuthenticationConfig> options)
	{
		ArgumentNullException.ThrowIfNull(options.Value.ApiKeys);
		(_next, _apiKeys) = (next, options.Value.ApiKeys);
	}
	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var requestApiKey))
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			await context.Response.WriteAsJsonAsync(ApiKeyMissingResponse);
			return;
		}

		if (!_apiKeys.Contains(requestApiKey))
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			await context.Response.WriteAsJsonAsync(UnauthorisedResponse);
			return;
		}

		await _next(context);
	}
}
