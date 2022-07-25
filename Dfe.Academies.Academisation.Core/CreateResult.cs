namespace Dfe.Academies.Academisation.Core;

public abstract class CreateResult<TPayload>
{
	protected CreateResult(ResultType resultType)
	{
		ResultType = resultType;
	}

	public ResultType ResultType { get; }
}
