using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetStakeholderObjectionsTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetStakeholderObjectionsTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsSectionFields()
	{
		var objection = SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided;
        var comment = "some comment";
        
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

		var request = new SetSignificantChangeStakeholderObjectionsPublicCommand(
			stakeholderObjections: objection,
			stakeholderObjectionsComment: comment);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetStakeholderObjections", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(objection, updated.Details.StakeholderObjections);
			Assert.Equal(comment, updated.Details.StakeholderObjectionsComment);
            Assert.Equal((byte)2, updated.Tier);
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