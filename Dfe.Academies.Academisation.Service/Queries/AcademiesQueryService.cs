using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IData.Http;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Academies;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academies.Contracts.V4.Establishments;
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

		public async Task<EstablishmentDto?> GetEstablishment(int urn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/v4/establishment/urn/{urn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishment failed for urn - {urn}, statuscode - {statusCode}", urn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<EstablishmentDto>();
		}

		public async Task<TrustDto?> GetTrust(string ukprn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/v4/trust/{ukprn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for trust failed for ukprn - {ukprn}, statuscode - {statusCode}", ukprn, response!.StatusCode);
				return null;
			}
			var trust = await response.Content.ReadFromJsonAsync<TrustDto>();

			return trust;
		}
	}
}
