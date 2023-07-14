using System.Data;
using System.Net;
using Dfe.Academies.Academisation.Domain.Exceptions;
using Dfe.Academies.Academisation.WebApi.ActionResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
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

			if (context.Exception.GetType() == typeof(ApplicationDomainException)
				&& context.Exception.InnerException?.GetType() == typeof(ValidationException))
			{
				var problemDetails = new ValidationProblemDetails()
				{
					Instance = context.HttpContext.Request.Path,
					Status = StatusCodes.Status400BadRequest,
					Detail = "Please refer to the errors property for additional details."
				};

				problemDetails.Errors.Add("DomainValidations", ((context.Exception.InnerException as ValidationException)!).Errors.Select(x => x.ErrorMessage.ToString()).ToArray());


				context.Result = new BadRequestObjectResult(problemDetails);
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			else if (context.Exception.GetType() == typeof(DbUpdateConcurrencyException))
			{
				context.Result = new ConflictObjectResult(context.Exception.Message);
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
			}
			else
			{

				var json = new JsonErrorResponse { Messages = new[] { "An error occurred.Try it again." } };

				if (env.IsDevelopment())
				{
					json.DeveloperMessage = context.Exception;
				}

				context.Result = new InternalServerErrorObjectResult(json);
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}

			context.ExceptionHandled = true;
		}

		private class JsonErrorResponse
		{
			public string[]? Messages { get; set; }

			public object? DeveloperMessage { get; set; }
		}
	}
}
