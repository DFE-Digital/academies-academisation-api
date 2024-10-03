using Dfe.Academisation.CorrelationIdMiddleware;

namespace Dfe.Academies.Academisation.Data.Http
{
	public interface ICompleteApiClientFactory
	{
		HttpClient Create(ICorrelationContext correlationContext);
	}
}
