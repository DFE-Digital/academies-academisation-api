using Dfe.Academisation.CorrelationIdMiddleware;

namespace Dfe.Academies.Academisation.IData.Http
{
	public interface IAcademiesApiClientFactory
	{
		HttpClient Create(ICorrelationContext correlationContext);
	}
}
