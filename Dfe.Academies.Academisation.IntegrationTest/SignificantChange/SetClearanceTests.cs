using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetClearanceTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory = factory;

	[Fact]
	public async Task Put_ReturnsOk_AndPersistsClearanceFields()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetClearance",
			new SetSignificantChangeClearancePublicCommand("Jane Smith", "John Doe"));

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("Jane Smith", updated.Details.RegionsGroupClearance);
			Assert.Equal("John Doe", updated.Details.TrustRegulationClearance);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeClearancePublicCommand("Jane Smith", "John Doe");

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetClearance", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
