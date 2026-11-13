using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SatelliteSiteArrangementTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
	[Theory]
	[InlineData(YesNoNotApplicableAnswer.No, 2, "Not a genuine expansion")]
	[InlineData(YesNoNotApplicableAnswer.Yes, 1, null)]
	[InlineData(YesNoNotApplicableAnswer.NotApplicable, 1, null)]
	public async Task Put_PersistsAssessmentAndTier_AndGetReturnsMappedTask(
		YesNoNotApplicableAnswer answer, byte expectedTier, string? expectedInformation)
	{
		var client = factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		factory.Context.Add(project);
		await factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SatelliteSiteArrangement",
			new SetSignificantChangeSatelliteSiteArrangementPublicCommand(answer, "Not a genuine expansion", "Evidence"));

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		factory.Context.ChangeTracker.Clear();
		var updated = await factory.Context.Set<SignificantChangeProject>()
			.Include(x => x.Details).SingleAsync(x => x.Id == project.Id);

		Assert.Equal(answer, updated.Details.SatelliteSiteArrangementAnswer);
		Assert.Equal(expectedInformation, updated.Details.SatelliteSiteArrangementAdditionalInformation);
		Assert.Equal("Evidence", updated.Details.SatelliteSiteArrangementSupportingEvidence);
		Assert.Equal(expectedTier, updated.Tier);

		var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
		options.Converters.Add(new JsonStringEnumConverter());
		var retrieved = await client.GetFromJsonAsync<SignificantChangeProjectGetByIdResponse>($"/significant-change/{project.Id}", options);

		Assert.NotNull(retrieved);
		Assert.Equal(answer, retrieved.SatelliteSiteArrangement.Answer);
		Assert.Equal(expectedInformation, retrieved.SatelliteSiteArrangement.AdditionalInformation);
		Assert.Equal("Evidence", retrieved.SatelliteSiteArrangement.SupportingEvidence);
		Assert.Equal(nameof(SignificantChangeTaskStatus.Completed), retrieved.SatelliteSiteArrangement.Status);
		Assert.Equal(expectedTier, retrieved.Tier);
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = factory.CreateClient();

		var response = await client.PutAsJsonAsync("/significant-change/99999/SatelliteSiteArrangement",
			new SetSignificantChangeSatelliteSiteArrangementPublicCommand(YesNoNotApplicableAnswer.Yes, null, null));

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
