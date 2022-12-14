namespace Dfe.Academies.Academisation.Core;

public class CreateValidationErrorResult : CreateResult
{
	public CreateValidationErrorResult(IEnumerable<ValidationError> validationErrors)
		: base(ResultType.ValidationError)
	{
		ValidationErrors = validationErrors.ToList().AsReadOnly();
	}

	public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

	public CreateValidationErrorResult MapToPayloadType()
	{
		return new CreateValidationErrorResult(ValidationErrors);
	}
}
