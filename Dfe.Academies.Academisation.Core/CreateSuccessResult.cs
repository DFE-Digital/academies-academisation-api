namespace Dfe.Academies.Academisation.Core;

public class CreateSuccessResult<TPayload> : CreateResult<TPayload>
{
	public CreateSuccessResult(TPayload payload) : base(ResultType.Success)
	{
		Payload = payload;
	}

	public TPayload Payload { get; }

	public CreateSuccessResult<TDestinationPayload> MapToPayloadType<TDestinationPayload>(Func<TPayload, TDestinationPayload> mapper)
	{
		return new CreateSuccessResult<TDestinationPayload>(mapper.Invoke(Payload));
	}
}
