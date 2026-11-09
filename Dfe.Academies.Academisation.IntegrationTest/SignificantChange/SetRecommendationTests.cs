using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetRecommendationTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory = factory;

	[Fact]
	public async Task Put_ReturnsOk_AndPersistsRecommendationFields()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetRecommendation",
			new SetSignificantChangeRecommendationPublicCommand(
				Recommendation.Decline,
				"This is the recommendation comment"));

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(Recommendation.Decline, updated.Details.Recommendation);
			Assert.Equal("This is the recommendation comment", updated.Details.RecommendationMoreInformation);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeRecommendationPublicCommand(
			Recommendation.Decline, "This is the recommendation comment");

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetRecommendation", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

}
