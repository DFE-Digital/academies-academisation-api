using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetReligiousBodyConsultationTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetReligiousBodyConsultationTests(TestWebApplicationFactory factory)
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

		var request = new SetSignificantChangeReligiousBodyConsultationPublicCommand(
			trustConsultedReligiousBody: YesNoNotApplicableAnswer.No,
			trustConsultedReligiousBodyNotConsultedReason: "Trust has not consulted religious bodies",
			religiousBodyRaisedObjection: YesNoNotApplicableAnswer.Yes,
			religiousBodyRaisedObjectionReason: "Religious body objection",
			religiousBodyConsentGranted: YesNoNotApplicableAnswer.NotApplicable,
			religiousBodyConsentNotGrantedReason: null,
			supportingEvidence: "https://example.com/evidence");

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetReligiousBodyConsultation", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)2, updated.Tier);
			Assert.Equal(YesNoNotApplicableAnswer.No, updated.Details.TrustConsultedReligiousBodyAnswer);
			Assert.Equal("Trust has not consulted religious bodies", updated.Details.TrustConsultedReligiousBodyNotConsultedReason);
			Assert.Equal(YesNoNotApplicableAnswer.Yes, updated.Details.ReligiousBodyRaisedObjection);
			Assert.Equal("Religious body objection", updated.Details.ReligiousBodyRaisedObjectionReason);
			Assert.Equal(YesNoNotApplicableAnswer.NotApplicable, updated.Details.ReligiousBodyConsentGranted);
			Assert.Equal("https://example.com/evidence", updated.Details.ReligiousBodySupportingEvidence);
		});
	}

	[Theory]
	[InlineData(null, SignificantChangeTaskStatus.InProgress)]
	[InlineData("  ", SignificantChangeTaskStatus.InProgress)]
	[InlineData("Details", SignificantChangeTaskStatus.Completed)]
	public async Task Put_SavesApplicableDetailsWithoutValidation_AndClearsThemWhenAnswersChange(
		string? text, SignificantChangeTaskStatus expectedStatus)
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetReligiousBodyConsultation",
			new SetSignificantChangeReligiousBodyConsultationPublicCommand(
				YesNoNotApplicableAnswer.No, text, YesNoNotApplicableAnswer.Yes, text, YesNoNotApplicableAnswer.No, text, null));

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);
		Assert.Equal(text, updated.Details.TrustConsultedReligiousBodyNotConsultedReason);
		Assert.Equal(text, updated.Details.ReligiousBodyRaisedObjectionReason);
		Assert.Equal(text, updated.Details.ReligiousBodyConsentNotGrantedReason);
		Assert.Equal(expectedStatus, updated.Details.GetReligiousBodyConsultationTaskStatus());

		response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetReligiousBodyConsultation",
			new SetSignificantChangeReligiousBodyConsultationPublicCommand(
				YesNoNotApplicableAnswer.Yes, "Ignore", YesNoNotApplicableAnswer.No, "Ignore",
				YesNoNotApplicableAnswer.NotApplicable, "Ignore", null));

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		_factory.Context.ChangeTracker.Clear();
		updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);
		Assert.Null(updated.Details.TrustConsultedReligiousBodyNotConsultedReason);
		Assert.Null(updated.Details.ReligiousBodyRaisedObjectionReason);
		Assert.Null(updated.Details.ReligiousBodyConsentNotGrantedReason);
		Assert.Equal(SignificantChangeTaskStatus.Completed, updated.Details.GetReligiousBodyConsultationTaskStatus());
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeReligiousBodyConsultationPublicCommand(
			trustConsultedReligiousBody: YesNoNotApplicableAnswer.Yes,
			trustConsultedReligiousBodyNotConsultedReason: null,
			religiousBodyRaisedObjection: YesNoNotApplicableAnswer.No,
			religiousBodyRaisedObjectionReason: "Objection details",
			religiousBodyConsentGranted: YesNoNotApplicableAnswer.Yes,
			religiousBodyConsentNotGrantedReason: null,
			supportingEvidence: null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetReligiousBodyConsultation", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
