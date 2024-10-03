using Dfe.Academisation.CorrelationIdMiddleware;

namespace Dfe.Academies.Academisation.Data.Http
{
	public class CompleteApiClientFactory : ICompleteApiClientFactory
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public CompleteApiClientFactory(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public HttpClient Create(ICorrelationContext correlationContext)
		{
			var httpClient = _httpClientFactory.CreateClient("CompleteApi");
			httpClient.DefaultRequestHeaders.Add(Keys.HeaderKey, correlationContext.CorrelationId.ToString());
			return httpClient;
		}
	}
}
