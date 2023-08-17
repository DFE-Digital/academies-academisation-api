using Dfe.Academisation.CorrelationIdMiddleware;

namespace Dfe.Academies.Academisation.WebApi.Middleware
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
		}

		public async Task Invoke(HttpContext context)
		{
			string? correlationId = null;

			if (context.Request.Headers.TryGetValue(Keys.HeaderKey, out var headerCorrelationId))
			{
				correlationId = headerCorrelationId;
			}

			using (_logger.BeginScope(new Dictionary<string, object>
			{
				["ApplicationId"] = Program.AppName,
				["CorrelationId"] = correlationId ?? string.Empty,
			}))
			{
				try
				{
					await _next(context);
				}
				finally
				{
					_logger.LogInformation(
						"Request {method} {url} => {statusCode}",
						context.Request?.Method,
						context.Request?.Path.Value,
						context.Response?.StatusCode);
				}
			}
		}
	}
}
