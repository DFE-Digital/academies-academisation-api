using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetConsultationDurationTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetConsultationDurationTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WhenDurationNotMet_ReturnsOk_PersistsFields_AndEscalatesToTierTwo()
	{
		var client = _factory.CreateClient();
		var project = CreateProject();

		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeConsultationDurationPublicCommand(
			consultationLastedMinimumThreeWeeks: ConsultationDurationAnswer.No,
			consultationDurationNotMetReason: "Consultation ran for two weeks only");

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetConsultationDuration", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)2, updated.Tier);
			Assert.Equal(ConsultationDurationAnswer.No, updated.Details.ConsultationLastedMinimumThreeWeeks);
			Assert.Equal("Consultation ran for two weeks only", updated.Details.ConsultationDurationNotMetReason);
		});
	}

	[Fact]
	public async Task Put_WhenSatisfactoryConsultationCarriedOut_ReturnsOk_AndStaysTierOne()
	{
		var client = _factory.CreateClient();
		var project = CreateProject();

		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var request = new SetSignificantChangeConsultationDurationPublicCommand(
			consultationLastedMinimumThreeWeeks: ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut,
			consultationDurationNotMetReason: null);

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetConsultationDuration", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)1, updated.Tier);
			Assert.Equal(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut, updated.Details.ConsultationLastedMinimumThreeWeeks);
			Assert.Null(updated.Details.ConsultationDurationNotMetReason);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeConsultationDurationPublicCommand(
			consultationLastedMinimumThreeWeeks: ConsultationDurationAnswer.Yes,
			consultationDurationNotMetReason: null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetConsultationDuration", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	private static SignificantChangeProject CreateProject()
	{
		return SignificantChangeProject.Create(
			urn: 123456,
			tier: 1,
			trustName: "Test Trust",
			trustUkprn: "12345678",
			route: "Change of age range",
			schoolName: "Test School",
			createdOn: DateTime.UtcNow);
	}
}