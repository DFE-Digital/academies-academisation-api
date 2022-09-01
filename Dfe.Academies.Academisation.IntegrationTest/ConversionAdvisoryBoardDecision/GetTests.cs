using System.Net;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.ConversionAdvisoryBoardDecision
{
	[Collection("AdvisoryBoardDecision")]
	public class GetTests
	{
		private readonly TestWebApplicationFactory _factory;

		public GetTests(TestWebApplicationFactory factory)
		{
			_factory = factory;
		}

		[Fact]
		public async void Get_WhenProjectFound___ReturnsOk_AndDecisionIsRetrievedFromDatabase()
		{
			const int conversionProjectId = 1000;
			var client = _factory.CreateClient();

			var result = await client.GetDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
				$"/conversion-project/advisory-board-decision/{conversionProjectId}");

			Assert.Multiple(() =>
			{
				Assert.Equal(HttpStatusCode.OK, result.StatusCode);
				Assert.NotNull(result.Result);
				Assert.Equal(conversionProjectId, result.Result.ConversionProjectId);
			});
		}

		[Fact]
		public async void Get_WhenProjectNotFound___ReturnsNotFound()
		{
			const int conversionProjectId = 9999;
			var client = _factory.CreateClient();

			var result = await client.GetDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
				$"/conversion-project/advisory-board-decision/{conversionProjectId}");
			
			Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
		}

		[Fact]
		public async void Get_WithoutApiKey_ReturnsUnauthorised()
		{
			var client = _factory.CreateClient();
			client.DefaultRequestHeaders.Remove("x-api-key");

			var result = await client.GetDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
				"/conversion-project/advisory-board-decision/1000");

			Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
		}
	}
}
 
