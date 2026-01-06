using System.Net.Http.Json;
using Dfe.Academies.Academisation.IService.Query; 
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Service.Extensions;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Service.Queries.Models;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class AcademiesQueryService(ILogger<AcademiesQueryService> logger, IAcademiesApiClientFactory academiesApiClientFactory, ICorrelationContext correlationContext) : IAcademiesQueryService
	{
		public async Task<EstablishmentDto?> GetEstablishmentByUkprn(string ukprn)
		{
			var client = academiesApiClientFactory.Create(correlationContext);
			var response = await client.GetAsync($"/v4/establishment/{ukprn}");

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for establishment failed for ukprn - {Ukprn}, statuscode - {StatusCode}", ukprn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<EstablishmentDto>();
		}

		public async Task<EstablishmentDto?> GetEstablishment(int urn)
		{
			var client = academiesApiClientFactory.Create(correlationContext);
			var response = await client.GetAsync($"/v4/establishment/urn/{urn}");

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for establishment failed for urn - {Urn}, statuscode - {StatusCode}", urn, response!.StatusCode);
				return null;
			}

			return await response.Content.ReadFromJsonAsync<EstablishmentDto>();
		}

		public async Task<TrustDto?> GetTrust(string ukprn)
		{
			var client = academiesApiClientFactory.Create(correlationContext);
			var response = await client.GetAsync($"/v4/trust/{ukprn}");

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for trust failed for ukprn - {Ukprn}, statuscode - {StatusCode}", ukprn, response!.StatusCode);
				return null;
			}
			var trust = await response.Content.ReadFromJsonAsync<TrustDto>();

			return trust;
		}

		public async Task<TrustDto?> GetTrustByReferenceNumber(string trustReferenceNumber)
		{
			var client = academiesApiClientFactory.Create(correlationContext);
			var response = await client.GetAsync($"/v4/trust/trustReferenceNumber/{trustReferenceNumber}");

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for trust failed for trustReferenceNumber - {TrustReferenceNumber}, statuscode - {StatusCode}", trustReferenceNumber, response!.StatusCode);
				return null;
			}
			var trust = await response.Content.ReadFromJsonAsync<TrustDto>();

			return trust;
		}

		public async Task<IEnumerable<EstablishmentDto>> GetBulkEstablishmentsByUkprn(IEnumerable<string> ukprns)
		{
			var client = academiesApiClientFactory.Create(correlationContext);

			string queryParameters = ukprns.Select(ukprn =>
			{
				return new KeyValuePair<string, string>("Ukprn", ukprn);
			})
			.ToList()
			.ToQueryString();

			var response = await client.GetAsync($"v4/establishments/ukprn/bulk{queryParameters}");

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for establishments failed , statuscode - {StatusCode}", response!.StatusCode);
				return null!;
			}
			var establishments = await response.Content.ReadFromJsonAsync<IEnumerable<EstablishmentDto>>();

			return establishments!;
		}

		public async Task<IEnumerable<EstablishmentDto>> PostBulkEstablishmentsByUkprns(IEnumerable<string> ukprns)
		{
			var client = academiesApiClientFactory.Create(correlationContext);

			// the model here
			var request = new UkprnRequestModel
			{ 
				Ukprns = ukprns
			};

			var response = await client.PostAsJsonAsync("v4/establishments/bulk/ukprns", request);

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for establishments failed , statuscode - {StatusCode}", response!.StatusCode);
				return null!;
			}

			var establishments = await response.Content.ReadFromJsonAsync<IEnumerable<EstablishmentDto>>();

			return establishments!;
		}

		public async Task<IEnumerable<EstablishmentDto>> PostBulkEstablishmentsByUrns(IEnumerable<int> urns)
		{
			var client = academiesApiClientFactory.Create(correlationContext);

			// the model here
			var request = new UrnRequestModel
			{
				Urns = urns
			};

			var response = await client.PostAsJsonAsync("v4/establishments/bulk/urns", request);

			if (!response.IsSuccessStatusCode)
			{
				logger.LogError("Request for establishments failed , statuscode - {StatusCode}", response!.StatusCode);
				return null!;
			}

			var establishments = await response.Content.ReadFromJsonAsync<IEnumerable<EstablishmentDto>>();

			return establishments!;
		}
	}
}
