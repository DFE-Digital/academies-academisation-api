using System;
using System.Net;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.ConversionAdvisoryBoardDecision;

[Collection("AdvisoryBoardDecision")]
public class PostTests
{
	private readonly TestWebApplicationFactory _factory;

	public PostTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async void Post_WhenRequestIsNotValid___ReturnsBadRequest_DoesNotUpdateDatabase()
	{
		//Arrange
		const int conversionProjectId = 9000;
		var client = _factory.CreateClient();
		var request = new AdvisoryBoardDecisionCreateRequestModel {ConversionProjectId = conversionProjectId};
		
		//Act
		var result = await client.PostAsJsonDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
			"/conversion-project/advisory-board-decision", request);
		
		var outcome = await client.GetDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
			$"/conversion-project/advisory-board-decision/{conversionProjectId}");

		//Assert
		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
			Assert.Equal(HttpStatusCode.NotFound, outcome.StatusCode);
		});

	}

	[Fact]
	public async void Post_WithValidRequest___ReturnsCreatedAndUpdatesDatabase()
	{
		//Arrange
		const int conversionProjectId = 9001;
		var client = _factory.CreateClient();
		var request = new AdvisoryBoardDecisionCreateRequestModel
		{
			ConversionProjectId = conversionProjectId,
			Decision = AdvisoryBoardDecision.Declined,
			ApprovedConditionsSet = null,
			ApprovedConditionsDetails = null,
			DeclinedReasons = new() {new(AdvisoryBoardDeclinedReason.Finance, "reason")},
			DeferredReasons = new(),
			AdvisoryBoardDecisionDate = DateTime.UtcNow.AddMonths(-1),
			DecisionMadeBy = DecisionMadeBy.RegionalDirectorForRegion
		};

		//Act
		var result = await client.PostAsJsonDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
			"/conversion-project/advisory-board-decision", request);

		var outcome = await client.GetDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
			$"/conversion-project/advisory-board-decision/{conversionProjectId}");

		//Assert
		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.Created, result.StatusCode);
			Assert.NotNull(result.Result);
			
			Assert.Equal(HttpStatusCode.OK, outcome.StatusCode);
			Assert.Equivalent(result.Result, outcome.Result);
		});
	}
		
	[Fact]
	public async void Post_WithoutApiKey_ReturnsUnauthorised()
	{
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Remove("x-api-key");

		var result = await client.PostAsync("/conversion-project/advisory-board-decision/1000", null);

		Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
	}
}
