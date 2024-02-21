using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IData.Http;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Academies;
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class AcademiesQueryService : IAcademiesQueryService
	{
		private readonly ILogger<AcademiesQueryService> _logger;
		private readonly IAcademiesApiClientFactory _academiesApiClientFactory;
		private readonly ICorrelationContext _correlationContext;

		public AcademiesQueryService(ILogger<AcademiesQueryService> logger, IAcademiesApiClientFactory academiesApiClientFactory, ICorrelationContext correlationContext)
		{
			_logger = logger;
			_academiesApiClientFactory = academiesApiClientFactory;
			_correlationContext = correlationContext;
		}

		public async Task<Establishment?> GetEstablishmentByUkprn(string ukprn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/establishment/{ukprn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishment failed for ukprn - {ukprn}, statuscode - {statusCode}", ukprn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<Establishment>();
		}

		public async Task<Establishment?> GetEstablishment(int urn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/establishment/urn/{urn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishment failed for urn - {urn}, statuscode - {statusCode}", urn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<Establishment>();
		}

		public async Task<Trust?> GetTrust(string ukprn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/v3/trusts?ukprn={ukprn}&includeEstablishments=false");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for trust failed for ukprn - {ukprn}, statuscode - {statusCode}", ukprn, response!.StatusCode);
				return null;
			}
			var trusts = await response.Content.ReadFromJsonAsync<AcademiesTrustsResponse>();

			return trusts?.Data.FirstOrDefault();
		}
	}
}
