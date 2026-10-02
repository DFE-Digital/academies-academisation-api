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

		var landTransactionApplication = SignificantChangeGenericYesNoNa.No;
		string landTransactionApplicationAdditionalInfo = "application details";
		var landTransactionConsent = SignificantChangeGenericYesNoNa.No;
		string landTransactionConsentAdditionalInfo = "consent details";
		string landTransactionSupportingEvidence = "evidence link";

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

		var request = new SetSignificantChangeLandTransactionPublicCommand(
			landTransactionApplication,
			landTransactionApplicationAdditionalInfo,
			landTransactionConsent,
			landTransactionConsentAdditionalInfo,
			landTransactionSupportingEvidence
        );

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetSignificantChangeLandTransaction", request);

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>()
			.SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal((byte)2, updated.Tier);
			Assert.Equal(landTransactionApplication, updated.Details.LandTransactionApplication);
			Assert.Equal(landTransactionApplicationAdditionalInfo, updated.Details.LandTransactionApplicationAdditionalInfo);
			Assert.Equal(landTransactionConsent, updated.Details.LandTransactionConsent);
			Assert.Equal(landTransactionConsentAdditionalInfo, updated.Details.LandTransactionConsentAdditionalInfo);
			Assert.Equal(landTransactionSupportingEvidence, updated.Details.LandTransactionSupportingEvidence);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeLandTransactionPublicCommand(
			SignificantChangeGenericYesNoNa.Yes,
			null,
			SignificantChangeGenericYesNoNa.Yes,
			null,
			null
        );

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetSignificantChangeLandTransaction", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}