namespace Dfe.Academies.Academisation.Core;

internal class CreateValidationErrorResult<T> : CreateResult<T>
{
	public CreateValidationErrorResult(IDictionary<string, IReadOnlyCollection<string>> validationErrors)
		: base(ResultType.ValidationError)
	{
		ValidationErrors = validationErrors;
	}

	public IDictionary<string, IReadOnlyCollection<string>> ValidationErrors { get; }
}
