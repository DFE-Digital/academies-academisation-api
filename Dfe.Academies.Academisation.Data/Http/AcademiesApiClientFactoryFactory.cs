using Dfe.Academies.Academisation.IData.Http;
using Dfe.Academisation.CorrelationIdMiddleware;

namespace Dfe.Academies.Academisation.Data.Http
{
	public class AcademiesApiClientFactoryFactory : IAcademiesApiClientFactory
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public AcademiesApiClientFactoryFactory(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public HttpClient Create(ICorrelationContext correlationContext)
		{
			var httpClient = _httpClientFactory.CreateClient("AcademiesApi");
			httpClient.DefaultRequestHeaders.Add(Dfe.Academisation.CorrelationIdMiddleware.Keys.HeaderKey, correlationContext.CorrelationId.ToString());
			return httpClient;
		}
	}
}
