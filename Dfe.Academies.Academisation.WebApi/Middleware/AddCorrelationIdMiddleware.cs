using Microsoft.Extensions.Primitives;

namespace Dfe.Academies.Academisation.WebApi.Middleware
{
	public class AddCorrelationIdMiddleware
	{
		public const string CorrelationIdHeaderKey = "X-Correlation-ID";
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		public AddCorrelationIdMiddleware(RequestDelegate next,
		ILoggerFactory loggerFactory)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_logger = loggerFactory.CreateLogger<AddCorrelationIdMiddleware>();
		}
		public async Task Invoke(HttpContext httpContext)
		{
			if (httpContext.Request.Headers.TryGetValue(
			CorrelationIdHeaderKey, out var correlationId))
			{
				_logger.LogInformation($"CorrelationId from Request Header:{ correlationId }");

			}
			else
			{
				correlationId = Guid.NewGuid().ToString();
				httpContext.Request.Headers.Add(CorrelationIdHeaderKey,
				correlationId);
				_logger.LogInformation($"Generated CorrelationId:{ correlationId}");

			}
			httpContext.Response.OnStarting(() =>
			{
				if (!httpContext.Response.Headers.
				TryGetValue(CorrelationIdHeaderKey,
				out correlationId))
					httpContext.Response.Headers.Add(
					CorrelationIdHeaderKey, correlationId);
				return Task.CompletedTask;
			});
			await _next.Invoke(httpContext);
		}
	}
}
