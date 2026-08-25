using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
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
		var request = new AdvisoryBoardDecisionCreateCommand { ConversionProjectId = conversionProjectId };

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
		var request = new AdvisoryBoardDecisionCreateCommand
		{
			ConversionProjectId = conversionProjectId,
			Decision = AdvisoryBoardDecision.Declined,
			ApprovedConditionsSet = null,
			ApprovedConditionsDetails = null,
			DeclinedReasons = new() { new(conversionProjectId, AdvisoryBoardDeclinedReason.Finance, "reason") },
			DeferredReasons = new(),
			WithdrawnReasons = new(),
			AdvisoryBoardDecisionDate = DateTime.UtcNow.AddMonths(-1),
			AcademyOrderDate = DateTime.UtcNow.AddMonths(-1),
			DecisionMadeBy = DecisionMadeBy.RegionalDirectorForRegion,
			DecisionMakerName = "John Smith"
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

	[Fact]
	public async Task Post_WithValidDaoNotIssueReason___ReturnsCreatedAndPersistsReason()
	{
		const int conversionProjectId = 9002;
		var client = _factory.CreateClient();
		var request = new AdvisoryBoardDecisionCreateCommand
		{
			ConversionProjectId = conversionProjectId,
			Decision = AdvisoryBoardDecision.DAONotIssued,
			DeclinedReasons = null,
			DAONotIssuedReasons =
			[
				new(0, AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy,
					"Projected outcomes show sustained unviability"),
				new(0, AdvisoryBoardDAONotIssuedReason.Other,
					"Some other reason")
			],
			DeferredReasons = null,
			WithdrawnReasons = null,
			AdvisoryBoardDecisionDate = DateTime.UtcNow.AddDays(-7),
			DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
			DecisionMakerName = "Jane Doe"
		};

		var result = await client.PostAsJsonDeserialized<ConversionAdvisoryBoardDecisionServiceModel>(
			"/conversion-project/advisory-board-decision", request);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.Created, result.StatusCode);
			Assert.NotNull(result.Result);
			Assert.Equal(AdvisoryBoardDecision.DAONotIssued, result.Result.Decision);
			Assert.NotNull(result.Result!.DAONotIssuedReasons);
			Assert.Equal(2, result.Result.DAONotIssuedReasons!.Count);

			Assert.Equal(AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy, result.Result.DAONotIssuedReasons[0].Reason);
			Assert.Equal("Projected outcomes show sustained unviability", result.Result.DAONotIssuedReasons[0].Details);

			Assert.Equal(AdvisoryBoardDAONotIssuedReason.Other, result.Result.DAONotIssuedReasons[1].Reason);
			Assert.Equal("Some other reason", result.Result.DAONotIssuedReasons[1].Details);
		});
	}
}
