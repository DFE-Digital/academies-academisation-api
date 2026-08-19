using System;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChangeDecision;

[Collection("AdvisoryBoardDecision")]
public class PutTests(TestWebApplicationFactory factory)
{
	[Fact]
	public async Task Put_WithValidRequest_ReturnsOkAndUpdatesDatabase()
	{
		const int significantChangeProjectId = 9001;
		var client = factory.CreateClient();

		var createRequest = new SignificantChangeDecisionCommand
		{
			SignificantChangeProjectId = significantChangeProjectId,
			Decision = Decision.Declined,
			ApprovedConditionsSet = null,
			ApprovedConditionsDetails = null,
			DeclinedReasons = [new(significantChangeProjectId, Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDeclinedReason.Finance, "initial")],
			DeferredReasons = [],
			WithdrawnReasons = [],
			DecisionDate = DateTime.UtcNow.AddMonths(-1),
			DecisionMadeBy = Domain.Core.ConversionAdvisoryBoardDecisionAggregate.DecisionMadeBy.RegionalDirectorForRegion,
			DecisionMakerName = "John Smith"
		};

		var createResult = await client.PostAsJsonDeserialized<SignificantChangeDecisionServiceModel>(
			"/significant-change/decision", createRequest);

		Assert.Equal(System.Net.HttpStatusCode.Created, createResult.StatusCode);
		Assert.NotNull(createResult.Result);

		var updateRequest = new SignificantChangeUpdateDecisionCommand
		{
			AdvisoryBoardDecisionId = createResult.Result!.AdvisoryBoardDecisionId,
			SignificantChangeProjectId = significantChangeProjectId,
			Decision = Decision.Approved,
			ApprovedConditionsSet = true,
			ApprovedConditionsDetails = "updated",
			DeclinedReasons = [],
			DeferredReasons = [],
			WithdrawnReasons = [],
			DecisionDate = DateTime.UtcNow,
			DecisionMadeBy = Domain.Core.ConversionAdvisoryBoardDecisionAggregate.DecisionMadeBy.DirectorGeneral,
			DecisionMakerName = "Jane Smith"
		};

		var response = await client.PutAsJsonAsync("/significant-change/decision", updateRequest);

		factory.Context.ChangeTracker.Clear();
		var decisions = await factory.Context
			.Set<Domain.ConversionAdvisoryBoardDecisionAggregate.ConversionAdvisoryBoardDecision>()
			.Include(conversionAdvisoryBoardDecision => conversionAdvisoryBoardDecision.AdvisoryBoardDecisionDetails)
			.ToListAsync();

		var updatedDecision = decisions.SingleOrDefault(x => x.Id == createResult.Result.AdvisoryBoardDecisionId);

		Assert.Multiple(() =>
		{
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(updatedDecision);
			Assert.Equal(Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Approved,
				updatedDecision!.AdvisoryBoardDecisionDetails.Decision);
			Assert.Equal("updated", updatedDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails);
		});
	}

	[Fact]
	public async Task Put_WithoutApiKey_ReturnsUnauthorised()
	{
		var client = factory.CreateClient();
		client.DefaultRequestHeaders.Remove("x-api-key");

		var response = await client.PutAsJsonAsync("/significant-change/decision", new SignificantChangeUpdateDecisionCommand());

		Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
	}
}
