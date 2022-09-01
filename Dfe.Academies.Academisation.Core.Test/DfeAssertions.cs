using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace Dfe.Academies.Academisation.Core.Test;

public class DfeAssertions
{
	public static void AssertCommandSuccess(CommandResult commandResult)
	{
		if (commandResult is CommandValidationErrorResult validationErrorResult)
		{
			throw new FailException("Validation Error:" + string.Join(";", validationErrorResult.ValidationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		Assert.IsAssignableFrom<CommandSuccessResult>(commandResult);
	}

	public static CreatedAtRouteResult AssertCreatedAtRoute<T>(ActionResult<T> result, string routeName)
	{
		if (result.Result is BadRequestObjectResult badRequestObjectResult)
		{
			var validationErrors = Assert.IsAssignableFrom<IReadOnlyCollection<ValidationError>>(badRequestObjectResult.Value);

			throw new FailException("BadObjectRequestResult" + string.Join(";", validationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		var createdAtRouteResult = Assert.IsAssignableFrom<CreatedAtRouteResult>(result.Result);
		Assert.IsAssignableFrom<T>(createdAtRouteResult.Value);
		Assert.Equal(routeName, createdAtRouteResult.RouteName);

		return createdAtRouteResult;
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

}
