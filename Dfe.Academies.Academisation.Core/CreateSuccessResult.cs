using AutoMapper;

namespace Dfe.Academies.Academisation.Core;

public class CreateSuccessResult<TPayload> : CreateResult
{
	public CreateSuccessResult(TPayload payload) : base(ResultType.Success)
	{
		Payload = payload;
	}

	public TPayload Payload { get; }

	// I don't like this but this overload will go away if everything is mapped using auto mapper, should probably have something that is specifically for applocation but it is bridging code
	public CreateSuccessResult<TDestinationPayload> MapToPayloadType<TDestinationPayload>(Func<TPayload, IMapper, TDestinationPayload> serviceModelMapper, IMapper mapper)
	{
		return new CreateSuccessResult<TDestinationPayload>(serviceModelMapper.Invoke(Payload, mapper));
	}

	public CreateSuccessResult<TDestinationPayload> MapToPayloadType<TDestinationPayload>(Func<TPayload, TDestinationPayload> serviceModelMapper)
	{
		return new CreateSuccessResult<TDestinationPayload>(serviceModelMapper.Invoke(Payload));
	}
}
