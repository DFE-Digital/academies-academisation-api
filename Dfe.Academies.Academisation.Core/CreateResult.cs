namespace Dfe.Academies.Academisation.Core;

public abstract class CreateResult<T>
{
	protected CreateResult(ResultType resultType)
	{
		ResultType = resultType;
	}

	public ResultType ResultType { get; }
}
