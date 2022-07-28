namespace Dfe.Academies.Academisation.Core;

public class CommandValidationErrorResult : CommandResult
{
	public CommandValidationErrorResult(IEnumerable<ValidationError> validationErrors)
	{
		ValidationErrors = validationErrors.ToList().AsReadOnly();
	}

	public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
}
