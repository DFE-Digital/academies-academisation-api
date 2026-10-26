using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetOtherStakeholderEngagementTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetOtherStakeholderEngagementTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_PersistsAllFields()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeOtherStakeholderEngagementPublicCommand(
			false,
			"Consultation details",
			true,
			"Objection details",
			YesNoNotApplicableAnswer.No,
			"Admissions notification details",
			"https://example.com/evidence");

		var response = await client.PutAsJsonAsync(
			$"/significant-change/{project.Id}/SetOtherStakeholderEngagement",
			request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(item => item.Id == project.Id);

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal((byte)2, updated.Tier);
		Assert.False(updated.Details.TrustConsultedStakeholders);
		Assert.Equal("Consultation details", updated.Details.TrustConsultedStakeholdersNotConsultedReason);
		Assert.True(updated.Details.WereThereAnyObjectionsRaised);
		Assert.Equal("Objection details", updated.Details.StakeholderObjectionsDetails);
		Assert.Equal(YesNoNotApplicableAnswer.No, updated.Details.RelevantStakeholdersNotifiedOfAdmissionsChanges);
		Assert.Equal("Admissions notification details", updated.Details.RelevantStakeholdersNotifiedOfAdmissionsChangesReason);
		Assert.Equal("https://example.com/evidence", updated.Details.OtherStakeholderEngagementSupportingEvidence);
	}

	[Fact]
	public async Task Put_WhenConditionalDetailsAreMissing_ReturnsBadRequest()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeOtherStakeholderEngagementPublicCommand(
			true,
			null,
			true,
			"  ",
			YesNoNotApplicableAnswer.Yes,
			null,
			null);

		var response = await client.PutAsJsonAsync(
			$"/significant-change/{project.Id}/SetOtherStakeholderEngagement",
			request);

		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeOtherStakeholderEngagementPublicCommand(
			true, null, false, null, YesNoNotApplicableAnswer.NotApplicable, null, null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetOtherStakeholderEngagement", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
