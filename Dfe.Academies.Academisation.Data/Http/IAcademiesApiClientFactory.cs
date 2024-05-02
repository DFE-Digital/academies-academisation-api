using Dfe.Academisation.CorrelationIdMiddleware;

namespace Dfe.Academies.Academisation.Data.Http
{
	public interface IAcademiesApiClientFactory
	{
		HttpClient Create(ICorrelationContext correlationContext);
	}
}
