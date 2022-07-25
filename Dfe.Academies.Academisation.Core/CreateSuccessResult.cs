namespace Dfe.Academies.Academisation.Core;

public class CreateSuccessResult<T> : CreateResult<T>
{
	public CreateSuccessResult(T payload) : base(ResultType.Success)
	{
		Payload = payload;
	}

	public T Payload { get; }
}
