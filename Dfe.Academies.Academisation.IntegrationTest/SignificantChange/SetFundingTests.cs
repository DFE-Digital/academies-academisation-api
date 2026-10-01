using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange;

public class SetFundingTests : IClassFixture<TestWebApplicationFactory>
{
	private readonly TestWebApplicationFactory _factory;

	public SetFundingTests(TestWebApplicationFactory factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task Put_ReturnsOk_AndPersistsFundingFields()
	{
		var client = _factory.CreateClient();
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
			DateTime.UtcNow);
		_factory.Context.Add(project);
		await _factory.Context.SaveChangesAsync();

		var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SetFunding",
			new SetSignificantChangeFundingPublicCommand(
				FundingAnswer.No,
				"Funding is unavailable",
				"Business case"));

		_factory.Context.ChangeTracker.Clear();
		var updated = await _factory.Context.Set<SignificantChangeProject>().SingleAsync(x => x.Id == project.Id);

		Assert.Multiple(() =>
		{
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(FundingAnswer.No, updated.Details.FundingAnswer);
			Assert.Equal("Funding is unavailable", updated.Details.FundingAdditionalInformation);
			Assert.Equal("Business case", updated.Details.FundingSupportingEvidence);
		});
	}

	[Fact]
	public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var client = _factory.CreateClient();
		var request = new SetSignificantChangeFundingPublicCommand(
			FundingAnswer.NotApplicable, null, null);

		var response = await client.PutAsJsonAsync("/significant-change/99999/SetFunding", request);

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

}
