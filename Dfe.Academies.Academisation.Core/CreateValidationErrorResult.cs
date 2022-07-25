namespace Dfe.Academies.Academisation.Core;

public class CreateValidationErrorResult<T> : CreateResult<T>
{
	public CreateValidationErrorResult(IEnumerable<ValidationError> validationErrors)
		: base(ResultType.ValidationError)
	{
		ValidationErrors = validationErrors.ToList().AsReadOnly();
	}

	public IReadOnlyCollection<ValidationError> ValidationErrors { get; }
}
