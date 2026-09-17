using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetLandTransactionConsentTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetLandTransactionConsentTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_WithValidRequest_ReturnsOk_AndPersistsSectionFields()
	{
		var client = _factory.CreateClient();

        var landTransactionConsentSecured = SignificantChangeLandTransactionConsent.No;
        string landTransactionConsentAdditionalInfo = "some additional info";

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

		var request = new SetSignificantChangeLandTransactionConsentPublicCommand(
			landTransactionConsentSecured,
			landTransactionConsentAdditionalInfo
        );

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetSignificantChangeLandTransactionConsent", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)2, updated.Tier);
			Assert.Equal(landTransactionConsentSecured, updated.Details.LandTransactionConsentSecured);
			Assert.Equal(landTransactionConsentAdditionalInfo, updated.Details.LandTransactionConsentAdditionalInfo);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeLandTransactionConsentPublicCommand(
			SignificantChangeLandTransactionConsent.Yes, 
            null
        );

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetSignificantChangeLandTransactionConsent", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}