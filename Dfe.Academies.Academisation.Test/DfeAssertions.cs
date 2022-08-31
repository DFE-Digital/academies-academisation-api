﻿using Dfe.Academies.Academisation.Core;
using Xunit;
using Xunit.Sdk;

namespace Dfe.Academies.Academisation.Test;

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
}