using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetEqualitiesImpactAssessmentTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetEqualitiesImpactAssessmentTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsSectionFields()
	{
		var client = _factory.CreateClient();
		var project = await CreateProjectAsync();

		var request = new SetSignificantChangeEqualitiesImpactAssessmentPublicCommand(
			EqualitiesImpactAssessmentCompleted: true,
			EqualitiesImpactIdentified: EqualitiesImpact.ImpactsIdentified,
			EqualitiesImpactIdentifiedMitigation: "Mitigation plan in place");

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetEqualitiesImpactAssessment", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.True(updated.Details.EqualitiesImpactAssessmentCompleted);
			Assert.Equal(EqualitiesImpact.ImpactsIdentified, updated.Details.EqualitiesImpactIdentified);
			Assert.Equal("Mitigation plan in place", updated.Details.EqualitiesImpactIdentifiedMitigation);
			Assert.Equal((byte)1, updated.Tier);
		});
	}

	[Fact]
	public async Task Put_WithNullValues_ReturnsOk_AndClearsSectionFields()
	{
		var client = _factory.CreateClient();
		var project = await CreateProjectAsync();
		project.SetEqualitiesImpactAssessment(true, EqualitiesImpact.PotentialImpacts, "Previous mitigation");
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeEqualitiesImpactAssessmentPublicCommand(
			EqualitiesImpactAssessmentCompleted: null,
			EqualitiesImpactIdentified: null,
			EqualitiesImpactIdentifiedMitigation: null);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetEqualitiesImpactAssessment", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Null(updated.Details.EqualitiesImpactAssessmentCompleted);
			Assert.Null(updated.Details.EqualitiesImpactIdentified);
			Assert.Null(updated.Details.EqualitiesImpactIdentifiedMitigation);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeEqualitiesImpactAssessmentPublicCommand(
			EqualitiesImpactAssessmentCompleted: true,
			EqualitiesImpactIdentified: EqualitiesImpact.None,
			EqualitiesImpactIdentifiedMitigation: null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetEqualitiesImpactAssessment", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	private async Task<SignificantChangeProject> CreateProjectAsync()
	{
		var project = SignificantChangeProject.Create(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			route: "Change of age range",
			schoolName: "Test School",
			createdOn: DateTime.UtcNow);

		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		return project;
	}
}
