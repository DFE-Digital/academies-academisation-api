namespace Dfe.Academies.Academisation.Core;

public class CreateValidationErrorResult<TPayload> : CreateResult<TPayload>
{
	public CreateValidationErrorResult(IEnumerable<ValidationError> validationErrors)
		: base(ResultType.ValidationError)
	{
		ValidationErrors = validationErrors.ToList().AsReadOnly();
	}

	public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

	public CreateValidationErrorResult<TDestinationPayload> MapToPayloadType<TDestinationPayload>()
	{
		return new CreateValidationErrorResult<TDestinationPayload>(ValidationErrors);
	}
}
