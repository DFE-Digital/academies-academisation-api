using System.Linq;
using Dfe.Academies.Academisation.WebApi.Controllers.Cypress;
using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.WebApi.Middleware
{
	public class CypressApiKeyMiddleware
	{
		private readonly ICypressKeyValidator _cypressKeyValidator;
		private const string ApiKeyHeader = "x-api-cypress-endpoints-key";
		private const string ApiKeyMissingResponse = "API key was not provided";
		private const string UnauthorisedResponse = "Unauthorised Client";
		private readonly RequestDelegate _next;

		public CypressApiKeyMiddleware(RequestDelegate next,
			ICypressKeyValidator cypressKeyValidator)
		{
			_next = next;
			_cypressKeyValidator = cypressKeyValidator;
		}

		/// <summary>
		/// Invokes the middleware checks, then the next middleware if everything fine.
		/// Only applies to endpoints for the cypress controller
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>A Task.</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			// If the controller being invoked is not the cypress endpoints controller, then this
			// middleware does not apply. Otherwise the key must match and be valid.
			if (context.Request.Path.StartsWithSegments(new PathString("cypress")))
			{

				if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var requestApiKey))
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					await context.Response.WriteAsJsonAsync(ApiKeyMissingResponse);
					return;
				}

				if (!_cypressKeyValidator.IsKeyValid(requestApiKey[0]!))
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					await context.Response.WriteAsJsonAsync(ApiKeyMissingResponse);
					return;
				}
			}

			await _next(context);
		}
	}
}
