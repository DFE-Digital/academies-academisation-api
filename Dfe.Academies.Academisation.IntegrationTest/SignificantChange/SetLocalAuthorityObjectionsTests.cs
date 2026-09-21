using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetLocalAuthorityObjectionsTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetLocalAuthorityObjectionsTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsSectionFields()
	{
		var client = _factory.CreateClient();

		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
				123456,
				1,
				"Test Trust",
				"12345678",
				"Change of age range",
				"Test School"),
			DateTime.UtcNow);

		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeLocalAuthorityObjectionsPublicCommand(
			localAuthorityRaisedObjections: true,
			localAuthorityObjectionsFurtherInformation: "Local authority has raised objections",
			supportingEvidenceLink: "https://example.org/evidence");

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetLocalAuthorityObjections", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)2, updated.Tier);
			Assert.True(updated.Details.LocalAuthorityRaisedObjections);
			Assert.Equal("Local authority has raised objections", updated.Details.LocalAuthorityObjectionsFurtherInformation);
			Assert.Equal("https://example.org/evidence", updated.Details.SupportingEvidenceLink);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();

		var existingProject = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
				123456,
				1,
				"Test Trust",
				"12345678",
				"Change of age range",
				"Test School"),
			DateTime.UtcNow);

		_factory.Context.Add(existingProject);
		await _factory.Context.SaveChangesAsync();

		var warmupRequest = new SetSignificantChangeLocalAuthorityObjectionsPublicCommand(
			localAuthorityRaisedObjections: false,
			localAuthorityObjectionsFurtherInformation: null,
			supportingEvidenceLink: null);

		await client.PutAsJsonAsync($"/significant-change/{existingProject.Id}/SetLocalAuthorityObjections", warmupRequest);

		var request = new SetSignificantChangeLocalAuthorityObjectionsPublicCommand(
			localAuthorityRaisedObjections: false,
			localAuthorityObjectionsFurtherInformation: null,
			supportingEvidenceLink: null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetLocalAuthorityObjections", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
