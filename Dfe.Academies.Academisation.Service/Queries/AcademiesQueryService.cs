﻿using System.Net.Http.Json;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Service.Extensions;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Service.Queries.Models;

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

		public async Task<EstablishmentDto?> GetEstablishmentByUkprn(string ukprn)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/v4/establishment/{ukprn}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishment failed for ukprn - {ukprn}, statuscode - {statusCode}", ukprn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<EstablishmentDto>();
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

		public async Task<TrustDto?> GetTrustByReferenceNumber(string trustReferenceNumber)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);
			var response = await client.GetAsync($"/v4/trust/trustReferenceNumber/{trustReferenceNumber}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for trust failed for trustReferenceNumber - {trustReferenceNumber}, statuscode - {statusCode}", trustReferenceNumber, response!.StatusCode);
				return null;
			}
			var trust = await response.Content.ReadFromJsonAsync<TrustDto>();

			return trust;
		}

		public async Task<IEnumerable<EstablishmentDto>> GetBulkEstablishmentsByUkprn(IEnumerable<string> ukprns)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);

			var queryParameters = ukprns.Select(ukprn =>
			{
				return new KeyValuePair<string, string>("Ukprn", ukprn);
			})
			.ToList()
			.ToQueryString();

			var response = await client.GetAsync($"v4/establishments/ukprn/bulk{queryParameters}");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishments failed , statuscode - {statusCode}", response!.StatusCode);
				return null!;
			}
			var establishments = await response.Content.ReadFromJsonAsync<IEnumerable<EstablishmentDto>>();

			return establishments!;
		}

		public async Task<IEnumerable<EstablishmentDto>> PostBulkEstablishmentsByUkprns(IEnumerable<string> ukprns)
		{
			var client = _academiesApiClientFactory.Create(_correlationContext);

			// the model here
			var request = new UkprnRequestModel
			{ 
				Ukprns = ukprns
			};

			var response = await client.PostAsJsonAsync("v4/establishments/bulk/ukprns", request);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError("Request for establishments failed , statuscode - {StatusCode}", response!.StatusCode);
				return null!;
			}

			var establishments = await response.Content.ReadFromJsonAsync<IEnumerable<EstablishmentDto>>();

			return establishments!;
		}
	}
}
