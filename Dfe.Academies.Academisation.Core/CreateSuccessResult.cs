namespace Dfe.Academies.Academisation.Core;

public class CreateSuccessResult<T> : CreateResult
{
	public CreateSuccessResult(T payload) : base(ResultType.Success)
	{
		Payload = payload;
	}

	public T Payload { get; }
}
