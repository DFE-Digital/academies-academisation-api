using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetOfstedInspectionTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetOfstedInspectionTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_ReturnsOk_AndPersistsOfstedInspectionFields()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();
		var inspectionDate = new DateTime(2026, 10, 20);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetOfstedInspection",
			new SetSignificantChangeOfstedInspectionPublicCommand(
				inspectionDate,
				false,
				"Standards were not met",
				"Inspection evidence"));

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(inspectionDate, updated.Details.OfstedInspectionDate);
			Assert.False(updated.Details.MeetsOfstedStandards);
			Assert.Equal("Standards were not met", updated.Details.OfstedAdditionalInformation);
			Assert.Equal("Inspection evidence", updated.Details.OfstedSupportingEvidence);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeOfstedInspectionPublicCommand(null, null, null, null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetOfstedInspection", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
