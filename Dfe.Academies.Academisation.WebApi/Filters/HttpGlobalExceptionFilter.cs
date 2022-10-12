using Dfe.Academies.Academisation.WebApi.ActionResults;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Net;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Dfe.Academies.Academisation.WebApi.Filters
{
	public class HttpGlobalExceptionFilter : IExceptionFilter
	{
		private readonly IHostingEnvironment env;
		private readonly ILogger<HttpGlobalExceptionFilter> logger;

		public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
		{
			this.env = env;
			this.logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			logger.LogError(new EventId(context.Exception.HResult),
				context.Exception,
				context.Exception.Message);

				var json = new JsonErrorResponse
				{
					Messages = new[] { "An error occurred.Try it again." }
				};

				if (env.IsDevelopment())
				{
					json.DeveloperMessage = context.Exception;
				}

				context.Result =  new InternalServerErrorObjectResult(json);
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			context.ExceptionHandled = true;
		}

		private class JsonErrorResponse
		{
			public string[]? Messages { get; set; }

			public object? DeveloperMessage { get; set; }
		}
	}
}
