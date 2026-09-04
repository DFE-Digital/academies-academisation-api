using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetProjectDatesTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsSectionFields()
	{
		var client = factory.CreateClient();

		var project = SignificantChangeProject.Create(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			route: "Change of age range",
			schoolName: "Test School",
			createdOn: DateTime.UtcNow);

		factory.Context.Add(project);
		await factory.Context.SaveChangesAsync();

		var proposedDecisionDate = new DateTime(2026, 9, 1);
		var proposedChangeDate = new DateTime(2027, 1, 1);

		var request = new SetSignificantChangeProjectDatesPublicCommand(
			ProposedDecisionDate: proposedDecisionDate,
			ProposedChangeDate: proposedChangeDate);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetSignificantChangeProjectDates", request);

		factory.Context.ChangeTracker.Clear();
		var updated = await factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(proposedDecisionDate, updated.Details.ProposedDecisionDate);
			Assert.Equal(proposedChangeDate, updated.Details.ProposedChangeDate);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = factory.CreateClient();
		var request = new SetSignificantChangeProjectDatesPublicCommand(
			ProposedDecisionDate: new DateTime(2026, 9, 1),
			ProposedChangeDate: new DateTime(2027, 1, 1));

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetSignificantChangeProjectDates", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
