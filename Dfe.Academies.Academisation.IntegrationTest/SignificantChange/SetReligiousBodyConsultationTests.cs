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
			religiousBodyRaisedObjection: YesNoNotApplicableAnswer.No,
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
			Assert.Equal(YesNoNotApplicableAnswer.No, updated.Details.ReligiousBodyRaisedObjection);
			Assert.Equal("Religious body objection", updated.Details.ReligiousBodyRaisedObjectionReason);
			Assert.Equal(YesNoNotApplicableAnswer.NotApplicable, updated.Details.ReligiousBodyConsentGranted);
			Assert.Equal("https://example.com/evidence", updated.Details.ReligiousBodySupportingEvidence);
		});
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

	[Fact]
	public async Task Put_WhenNoAnswerHasNoReason_ReturnsBadRequest()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeReligiousBodyConsultationPublicCommand(
			YesNoNotApplicableAnswer.No,
			null,
			YesNoNotApplicableAnswer.Yes,
			null,
			YesNoNotApplicableAnswer.NotApplicable,
			null,
			null);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetReligiousBodyConsultation", request);

		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}
}
