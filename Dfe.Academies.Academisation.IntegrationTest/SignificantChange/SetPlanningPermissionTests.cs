using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetPlanningPermissionTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
	[Fact]
	public async Task Put_WhenPlanningPermissionIsProvided_ReturnsOk_PersistsFields()
	{
		var client = factory.CreateClient();
		var project = CreateProject();

		factory.Context.Add(project);
		await factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangePlanningPermissionPublicCommand(
			PlanningPermissionAnswer: PlanningPermissionAnswer.Yes,
			AdditionalInformation: "Planning permission granted",
			SupportingEvidence: "Evidence attached");

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetPlanningPermission", request);

		factory.Context.ChangeTracker.Clear();
		var updated = await factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(PlanningPermissionAnswer.Yes, updated.Details.PlanningPermission);
			Assert.Equal("Planning permission granted", updated.Details.PlanningPermissionAdditionalInformation);
			Assert.Equal("Evidence attached", updated.Details.PlanningPermissionSupportingEvidence);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = factory.CreateClient();
		var request = new SetSignificantChangePlanningPermissionPublicCommand(
			PlanningPermissionAnswer: PlanningPermissionAnswer.NotApplicable,
			AdditionalInformation: null,
			SupportingEvidence: null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetPlanningPermission", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	private static SignificantChangeProject CreateProject()
	{
		return SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
				urn: 123456,
				tier: 1,
				trustName: "Test Trust",
				trustUkprn: "12345678",
				typeOfSignificantChange: "Change of age range",
				schoolName: "Test School"),
			createdOn: DateTime.UtcNow);
	}
}
