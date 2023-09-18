using System.Net.Http.Json;
using Dfe.Academies.Academisation.IData.Establishment;
using Dfe.Academies.Academisation.IData.Http;
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Data.Establishment
{
	public class EstablishmentGetDataQuery : IEstablishmentGetDataQuery
	{
		private readonly ILogger<EstablishmentGetDataQuery> _logger;
		private readonly IAcademiesApiClientFactory _academiesApiClientFactory;
		private readonly ICorrelationContext _correlationContext;

		public EstablishmentGetDataQuery(ILogger<EstablishmentGetDataQuery> logger, IAcademiesApiClientFactory academiesApiClientFactory, ICorrelationContext correlationContext)
		{
			_logger = logger;
			_academiesApiClientFactory = academiesApiClientFactory;
			_correlationContext = correlationContext;
		}

		public async Task<IData.Establishment.Establishment?> GetEstablishment(int urn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			 var response = await client.GetAsync($"/establishment/urn/{urn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishment failed for urn - {urn}, statuscode - {statusCode}", urn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<IData.Establishment.Establishment>();
		}
	}
}
