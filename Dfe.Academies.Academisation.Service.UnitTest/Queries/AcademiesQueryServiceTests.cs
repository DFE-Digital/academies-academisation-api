using System.Net;
using AutoFixture;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Service.Queries;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academisation.CorrelationIdMiddleware;
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
			this._mockRepository = new MockRepository(MockBehavior.Default);
			this._mockLogger = this._mockRepository.Create<ILogger<AcademiesQueryService>>();

			this._correlationContext = new CorrelationContext();
			_correlationContext.SetContext(Guid.NewGuid());

			var httpClient = _mockHttpMessageHandler.ToHttpClient();
			httpClient.BaseAddress = new Uri(baseUri);

			_academiesApiClientFactory = new Mock<IAcademiesApiClientFactory>();
			_academiesApiClientFactory.Setup(x => x.Create(_correlationContext)).Returns(httpClient);

			this._service = new AcademiesQueryService(this._mockLogger.Object, _academiesApiClientFactory.Object, this._correlationContext);
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

			var result = await this._service.PostBulkEstablishmentsByUkprns(["1234", "324"]);

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

			var result = await this._service.PostBulkEstablishmentsByUkprns(["1234", "324"]);

			Assert.Null(result);
		}
	}
}
