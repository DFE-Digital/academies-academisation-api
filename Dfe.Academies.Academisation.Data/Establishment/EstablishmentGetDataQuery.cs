using System.Net.Http.Json;
using Dfe.Academies.Academisation.IData.Establishment;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Data.Establishment
{
	public class EstablishmentGetDataQuery : IEstablishmentGetDataQuery
	{
		private readonly ILogger<EstablishmentGetDataQuery> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public EstablishmentGetDataQuery(ILogger<EstablishmentGetDataQuery> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
		}

		public async Task<IData.Establishment.Establishment?> GetEstablishment(int urn)
		{
			var httpClient = _httpClientFactory.CreateClient("AcademiesApi");
			var response = await httpClient.GetAsync($"/establishment/urn/{urn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishment failed for urn - {urn}, statuscode - {statusCode}", urn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<IData.Establishment.Establishment>();
		}
	}
}
