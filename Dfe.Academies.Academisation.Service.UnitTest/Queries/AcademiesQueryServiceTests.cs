using System.Net;
using AutoFixture;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Service.Extensions;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academisation.CorrelationIdMiddleware;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
    public class AcademiesQueryServiceTests
    {
		private readonly MockRepository _mockRepository;
		private readonly MockHttpMessageHandler _mockHttpMessageHandler = new();
		private readonly Fixture _fixture = new();

		private readonly Mock<ILogger<AcademiesQueryService>> _mockLogger;
		private readonly Mock<IAcademiesApiClientFactory> _academiesApiClientFactory;

		private readonly CorrelationContext _correlationContext;

		private readonly AcademiesQueryService _service;

		private readonly string baseUri = "http://localhost";

		public AcademiesQueryServiceTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Default);
			_mockLogger = _mockRepository.Create<ILogger<AcademiesQueryService>>();

			_correlationContext = new CorrelationContext();
			_correlationContext.SetContext(Guid.NewGuid());

			var httpClient = _mockHttpMessageHandler.ToHttpClient();
			httpClient.BaseAddress = new Uri(baseUri);

			_academiesApiClientFactory = new Mock<IAcademiesApiClientFactory>();
			_academiesApiClientFactory.Setup(x => x.Create(_correlationContext)).Returns(httpClient);

			_service = new AcademiesQueryService(_mockLogger.Object, _academiesApiClientFactory.Object, _correlationContext);
		}

		[Fact]
		public async Task PostBulkEstablishmentsByUkprns_ShouldReturn_ListOf_Establishments()
		{
			var establishment1 = _fixture.Create<EstablishmentDto>();
			var establishment2 = _fixture.Create<EstablishmentDto>();
			var list = new List<EstablishmentDto>
			{
				establishment1,
				establishment2
			};

			_mockHttpMessageHandler.When($"{baseUri}/v4/establishments/bulk/ukprns")
					.Respond("application/json", JsonConvert.SerializeObject(list));

			var result = await _service.PostBulkEstablishmentsByUkprns(["1234", "324"]);

			Assert.Equal(2, result.ToList().Count);

			var establishment = result.SingleOrDefault(x => x.Ukprn == list[0].Ukprn);
			Assert.NotNull(establishment);

			establishment = result.SingleOrDefault(x => x.Ukprn == list[1].Ukprn);
			Assert.NotNull(establishment);
		}

		[Fact]
		public async Task PostBulkEstablishmentsByUkprns_ShouldReturn_Null_When_NotFound()
		{
			_mockHttpMessageHandler.When($"{baseUri}/v4/establishments/bulk/ukprns").Respond(HttpStatusCode.NotFound);

			var result = await _service.PostBulkEstablishmentsByUkprns(["1234", "324"]);

			Assert.Null(result);
		}
		[Fact]
		public async Task PostBulkEstablishmentsByUrns_ShouldReturn_Null_When_NotFound()
		{
			_mockHttpMessageHandler.When($"{baseUri}/v4/establishments/bulk/urns").Respond(HttpStatusCode.NotFound);

			var result = await _service.PostBulkEstablishmentsByUrns([1234, 324]);

			Assert.Null(result);
		}
		[Fact]
		public async Task PostBulkEstablishmentsByUrns_ShouldReturn_ListOf_Establishments()
		{
			var establishment1 = _fixture.Create<EstablishmentDto>();
			var establishment2 = _fixture.Create<EstablishmentDto>();
			var list = new List<EstablishmentDto>
			{
				establishment1,
				establishment2
			};

			_mockHttpMessageHandler.When($"{baseUri}/v4/establishments/bulk/urns")
					.Respond("application/json", JsonConvert.SerializeObject(list));

			var result = await _service.PostBulkEstablishmentsByUrns([1234, 324]);

			Assert.Equal(2, result.ToList().Count);

			var establishment = result.SingleOrDefault(x => x.Ukprn == list[0].Ukprn);
			Assert.NotNull(establishment);

			establishment = result.SingleOrDefault(x => x.Ukprn == list[1].Ukprn);
			Assert.NotNull(establishment);
		}

		[Fact]
		public async Task GetBulkEstablishmentsByUkprn_ShouldReturn_Null_When_NotFound()
		{
			var ukprns = new List<string> { "1234" };
			string queryParameters = ukprns.Select(ukprn =>
			{
				return new KeyValuePair<string, string>("Ukprn", ukprn);
			})
			.ToList()
			.ToQueryString();
			_mockHttpMessageHandler.When($"{baseUri}/v4/establishments/ukprn/bulk{queryParameters}").Respond(HttpStatusCode.NotFound);

			var result = await _service.GetBulkEstablishmentsByUkprn(ukprns);

			Assert.Null(result);
		}
		[Fact]
		public async Task GetBulkEstablishmentsByUkprn_ShouldReturn_ListOf_Establishments()
		{
			var ukprns = new List<string> { "1234" };
			string queryParameters = ukprns.Select(ukprn =>
			{
				return new KeyValuePair<string, string>("Ukprn", ukprn);
			})
			.ToList()
			.ToQueryString();
			var establishment1 = _fixture.Create<EstablishmentDto>();
			var list = new List<EstablishmentDto>
			{
				establishment1
			};
			_mockHttpMessageHandler.When($"{baseUri}/v4/establishments/ukprn/bulk{queryParameters}").Respond("application/json", JsonConvert.SerializeObject(list));

			var result = await _service.GetBulkEstablishmentsByUkprn(ukprns);

			Assert.Single(result.ToList());

			var establishment = result.SingleOrDefault(x => x.Ukprn == list[0].Ukprn);
			Assert.NotNull(establishment); 
		}
		[Fact]
		public async Task GetTrustByReferenceNumber_ShouldReturn_Null_When_NotFound()
		{
			string trustReferenceNumber = "1234";
			_mockHttpMessageHandler.When($"{baseUri}/v4/trust/trustReferenceNumber/{trustReferenceNumber}").Respond(HttpStatusCode.NotFound);

			var result = await _service.GetTrustByReferenceNumber(trustReferenceNumber);

			Assert.Null(result);
		}
		[Fact]
		public async Task GetTrustByReferenceNumber_ShouldReturn_ListOf_Establishments()
		{
			string trustReferenceNumber = "1234"; 
			_mockHttpMessageHandler.When($"{baseUri}/v4/trust/trustReferenceNumber/{trustReferenceNumber}").Respond("application/json", JsonConvert.SerializeObject(_fixture.Create<TrustDto>()));

			var result = await _service.GetTrustByReferenceNumber(trustReferenceNumber);

			Assert.NotNull(result);
		}
	}
}
