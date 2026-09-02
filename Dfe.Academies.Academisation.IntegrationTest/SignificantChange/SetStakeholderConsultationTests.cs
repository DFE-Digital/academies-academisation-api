using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetStakeholderConsultationTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetStakeholderConsultationTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsSectionFields()
	{
		var client = _factory.CreateClient();

		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			typeOfSignificantChange: "Change of age range",
			schoolName: "Test School"),
			createdOn: DateTime.UtcNow);

		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeStakeholderConsultationPublicCommand(
			trustConsultedStakeholders: false,
			trustConsultedStakeholdersNotConsultedReason: "Trust has not consulted stakeholders yet");

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetStakeholderConsultation", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)2, updated.Tier);
			Assert.False(updated.Details.TrustConsultedStakeholders);
			Assert.Equal("Trust has not consulted stakeholders yet", updated.Details.TrustConsultedStakeholdersNotConsultedReason);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeStakeholderConsultationPublicCommand(
			trustConsultedStakeholders: true,
			trustConsultedStakeholdersNotConsultedReason: null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetStakeholderConsultation", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}