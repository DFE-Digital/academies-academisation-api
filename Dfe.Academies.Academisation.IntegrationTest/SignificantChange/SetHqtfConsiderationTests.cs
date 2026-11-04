using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetHqtfConsiderationTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetHqtfConsiderationTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_ReturnsOk_AndPersistsHqtfConsiderationFields()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetHQTFConsideration",
			new SetSignificantChangeHqtfConsiderationPublicCommand(
				HqtfConsiderationAnswer.Yes,
				"HQTF not applicable",
				"Evidence document"));

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(HqtfConsiderationAnswer.Yes, updated.Details.HqtfConsiderationAnswer);
			Assert.Equal("HQTF not applicable", updated.Details.HqtfConsiderationAdditionalInformation);
			Assert.Equal("Evidence document", updated.Details.HqtfConsiderationSupportingEvidence);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeHqtfConsiderationPublicCommand(
			HqtfConsiderationAnswer.NotApplicable, null, null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetHQTFConsideration", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
