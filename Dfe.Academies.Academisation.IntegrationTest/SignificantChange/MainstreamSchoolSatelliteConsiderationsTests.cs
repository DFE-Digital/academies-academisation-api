using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IntegrationTest.Extensions;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class MainstreamSchoolSatelliteConsiderationsTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
	[Theory]
	[InlineData(YesNoNotApplicableAnswer.No, "Requirements gap", 2)]
	[InlineData(YesNoNotApplicableAnswer.Yes, null, 1)]
	[InlineData(YesNoNotApplicableAnswer.NotApplicable, null, 1)]
	public async Task Put_PersistsFieldsAndTier_AndGetReturnsTask(YesNoNotApplicableAnswer answer, string? expectedDetails, int expectedTier)
	{
		var client = factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		factory.Context.Add(project);
		await factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/MainstreamSchoolSatelliteConsiderations",
			new SetSignificantChangeMainstreamSchoolSatelliteConsiderationsPublicCommand(answer, "Requirements gap", "Evidence"));

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		factory.Context.ChangeTracker.Clear();
		var updated = await factory.Context.Set<SignificantChangeProject>()
			.Include(x => x.Details).SingleAsync(x => x.Id == project.Id);

		Assert.Equal(answer, updated.Details.MainstreamSchoolSatelliteConsiderationsMeetRequirements);
		Assert.Equal(expectedDetails, updated.Details.MainstreamSchoolSatelliteConsiderationsAdditionalInformation);
		Assert.Equal("Evidence", updated.Details.MainstreamSchoolSatelliteConsiderationsSupportingEvidence);
		Assert.Equal(expectedTier, updated.Tier);

		var (statusCode, result) = await client.GetDeserialized<SignificantChangeProjectGetByIdResponse>($"/significant-change/{project.Id}");
		Assert.Equal(HttpStatusCode.OK, statusCode);
		Assert.NotNull(result);
		Assert.Equal(answer, result.MainstreamSchoolSatelliteConsiderations.Answer);
		Assert.Equal(expectedDetails, result.MainstreamSchoolSatelliteConsiderations.AdditionalInformation);
		Assert.Equal("Evidence", result.MainstreamSchoolSatelliteConsiderations.SupportingEvidence);
		Assert.Equal(nameof(SignificantChangeTaskStatus.Completed), result.MainstreamSchoolSatelliteConsiderations.Status);
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = factory.CreateClient();

		var response = await client.PutAsJsonAsync("/significant-change/99999/MainstreamSchoolSatelliteConsiderations",
			new SetSignificantChangeMainstreamSchoolSatelliteConsiderationsPublicCommand(YesNoNotApplicableAnswer.Yes, null, null));

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
