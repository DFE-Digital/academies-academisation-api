using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetAssignedUserTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetAssignedUserTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsAssignedUser()
	{
		var client = _factory.CreateClient();

		var project = SignificantChangeProject.Create(
			urn: 123456,
			tier: 2,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			route: "Change of age range",
			schoolName: "Test School",
			createdOn: DateTime.UtcNow);

		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var expectedUserId = Guid.NewGuid();
		var expectedName = "Assigned User";
		var expectedEmail = "assigned.user@test.local";
		var request = new SetSignificantChangeAssignedUserPublicCommand(
			userId: expectedUserId,
			fullName: expectedName,
			emailAddress: expectedEmail);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetAssignedUser", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(expectedUserId, updated.AssignedUserId);
			Assert.Equal(expectedName, updated.AssignedUserFullName);
			Assert.Equal(expectedEmail, updated.AssignedUserEmailAddress);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeAssignedUserPublicCommand(
			userId: Guid.NewGuid(),
			fullName: "Assigned User",
			emailAddress: "assigned.user@test.local");

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetAssignedUser", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}