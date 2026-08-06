using System;
using System.Linq;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChangeDecision;

[Collection("AdvisoryBoardDecision")]
public class PostTests(TestWebApplicationFactory factory)
{
	[Fact]
	public async Task Post_WhenRequestIsNotValid_ReturnsBadRequest_DoesNotUpdateDatabase()
	{
		const int significantChangeProjectId = 9000;
		var client = factory.CreateClient();
		var request = new SignificantChangeDecisionCommand { SignificantChangeProjectId = significantChangeProjectId };

		var result = await client.PostAsJsonDeserialized<SignificantChangeDecisionServiceModel>(
			"/significant-change/decision", request);

		factory.Context.ChangeTracker.Clear();
		var decisions = await factory.Context
			.Set<Domain.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecision>()
			.Include(conversionAdvisoryBoardDecision => conversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails).ToListAsync();
		
		var outcome = decisions.SingleOrDefault(x =>
			x.AdvisoryBoardDecisionDetails.SignificantChangeProjectId == significantChangeProjectId);

		Assert.Multiple(() =>
		{
			Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
			Assert.Null(outcome);
		});
	}

	[Fact]
	public async Task Post_WithValidRequest_ReturnsCreatedAndUpdatesDatabase()
	{
		//Arrange
		const int significantChangeProjectId = 9001;
		var client = factory.CreateClient();
		var request = new SignificantChangeDecisionCommand
		{
			SignificantChangeProjectId = significantChangeProjectId,
			Decision = Decision.Declined,
			ApprovedConditionsSet = null,
			ApprovedConditionsDetails = null,
			DeclinedReasons = [new(significantChangeProjectId, AdvisoryBoardDeclinedReason.Finance, "reason")],
			DeferredReasons = [],
			WithdrawnReasons = [],
			DecisionDate = DateTime.UtcNow.AddMonths(-1),
			DecisionMadeBy = DecisionMadeBy.RegionalDirectorForRegion,
			DecisionMakerName = "John Smith"
		};

		var result = await client.PostAsJsonDeserialized<SignificantChangeDecisionServiceModel>(
			"/significant-change/decision", request);

		factory.Context.ChangeTracker.Clear();
		var decisions = await factory.Context
			.Set<Domain.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecision>()
			.Include(conversionAdvisoryBoardDecision => conversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails).ToListAsync();
		
		var outcome = decisions.SingleOrDefault(x =>
			x.AdvisoryBoardDecisionDetails.SignificantChangeProjectId == significantChangeProjectId);

		Assert.Multiple(() =>
		{
			Assert.Equal(System.Net.HttpStatusCode.Created, result.StatusCode);
			Assert.NotNull(result.Result);

			Assert.NotNull(outcome);
			Assert.Equal(significantChangeProjectId, outcome.AdvisoryBoardDecisionDetails.SignificantChangeProjectId);
			Assert.Equal(AdvisoryBoardDecision.Declined, outcome.AdvisoryBoardDecisionDetails.Decision);
		});
	}

	[Fact]
	public async Task Post_WithoutApiKey_ReturnsUnauthorised()
	{
		var client = factory.CreateClient();
		client.DefaultRequestHeaders.Remove("x-api-key");

		var result = await client.PostAsync("/significant-change/decision", null);

		Assert.Equal(System.Net.HttpStatusCode.Unauthorized, result.StatusCode);
	}
}
