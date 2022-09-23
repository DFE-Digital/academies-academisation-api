using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace Dfe.Academies.Academisation.Core.Test;

public class DfeAssert
{
	public static void CommandSuccess(CommandResult commandResult)
	{
		if (commandResult is CommandValidationErrorResult validationErrorResult)
		{
			throw new FailException("Validation Error:" + string.Join(";", validationErrorResult.ValidationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		Assert.IsAssignableFrom<CommandSuccessResult>(commandResult);
	}

	public static void CommandValidationError(CommandResult commandResult, string propertyName)
	{
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(commandResult);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Contains(propertyName, error.PropertyName);
	}

	public static void CommandValidationError(CommandResult commandResult, string propertyName, string errorMessage)
	{
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(commandResult);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Contains(propertyName, error.PropertyName);
		Assert.Contains(errorMessage, error.ErrorMessage);
	}

	public static Tuple<CreatedAtRouteResult, T> CreatedAtRoute<T>(ActionResult<T> result, string routeName)
	{
		if (result.Result is BadRequestObjectResult badRequestObjectResult)
		{
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestObjectResult.Value);

			throw new FailException("BadObjectRequestResult" + string.Join(";", validationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		var createdAtRouteResult = Assert.IsAssignableFrom<CreatedAtRouteResult>(result.Result);
		var payload = Assert.IsAssignableFrom<T>(createdAtRouteResult.Value);
		Assert.Equal(routeName, createdAtRouteResult.RouteName);

		return Tuple.Create(createdAtRouteResult, payload);
	}

	public static OkResult OkResult(ActionResult result)
	{
		if (result is BadRequestObjectResult badRequestObjectResult)
		{
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestObjectResult.Value);

			throw new FailException("BadObjectRequestResult: " + string.Join(";", validationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		var okResult = Assert.IsAssignableFrom<OkResult>(result);

		return okResult;
	}

	public static Tuple<OkObjectResult, T> OkObjectResult<T>(ActionResult<T> result)
	{
		Assert.NotNull(result.Result);

		if (result.Result is BadRequestObjectResult badRequestObjectResult)
		{
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestObjectResult.Value);

			throw new FailException("BadObjectRequestResult: " + string.Join(";", validationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		var okResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
		var payload = Assert.IsAssignableFrom<T>(okResult.Value);

		return Tuple.Create(okResult, payload);
	}

	public static BadRequestObjectResult BadRequestObjectResult(ActionResult result, string propertyName)
	{
		var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
		var errors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestObjectResult.Value);
		var error = Assert.Single(errors);
		Assert.NotNull(error);
		Assert.Equal(propertyName, error.PropertyName);

		return badRequestObjectResult;
	}
}
