using System.Net.Http.Json;
using Dfe.Academies.Academisation.IData.Establishment;

namespace Dfe.Academies.Academisation.Data.Establishment
{
	public class EstablishmentGetDataQuery : IEstablishmentGetDataQuery
	{
		private readonly HttpClient _httpClient;

		public EstablishmentGetDataQuery(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("AcademiesApi");
		}

		public async Task<IData.Establishment.Establishment?> GetEstablishment(int urn)
		{
			var response = await _httpClient.GetAsync($"/establishment/urn/{urn}");

			if (response == null || !response.IsSuccessStatusCode)
			{
				// Log something
				return new IData.Establishment.Establishment();
			}

			return await response.Content.ReadFromJsonAsync<IData.Establishment.Establishment>();
		}
	}
}
