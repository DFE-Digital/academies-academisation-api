
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Dfe.Academies.Academisation.IntegrationTest.SignificantChange
{
	public class SpecialSchoolSatelliteConsiderationsTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
	{
		[Fact]
		public async Task Put_ReturnsOk_AndPersistsSpecialSchoolSatelliteConsiderationsFields()
		{
			var client = factory.CreateClient();
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
				DateTime.UtcNow);
			factory.Context.Add(project);
			await factory.Context.SaveChangesAsync();

			var response = await client.PutAsJsonAsync($"/significant-change/{project.Id}/SpecialSchoolSatelliteConsiderations",
				new SetSignificantChangeSpecialSchoolSatelliteConsiderationsPublicCommand(
					YesNoNotApplicableAnswer.No,
					"Does not meet",
					"Evidence"));

			factory.Context.ChangeTracker.Clear();
			var updated = await factory.Context.Set<SignificantChangeProject>()
				.Include(significantChangeProject => significantChangeProject.Details).SingleAsync(x => x.Id == project.Id);

			Assert.Multiple(() =>
			{
				Assert.Equal(HttpStatusCode.OK, response.StatusCode);
				Assert.Equal(YesNoNotApplicableAnswer.No, updated.Details.SpecialSchoolSatelliteConsiderationsMeetRequirements);
				Assert.Equal("Does not meet", updated.Details.SpecialSchoolSatelliteConsiderationsAdditionalInformation);
				Assert.Equal("Evidence", updated.Details.SpecialSchoolSatelliteConsiderationsSupportingEvidence);
			});
		}

		[Fact]
		public async Task Put_WhenProjectDoesNotExist_ReturnsNotFound()
		{
			var client = factory.CreateClient();
			var request = new SetSignificantChangeSpecialSchoolSatelliteConsiderationsPublicCommand(
				YesNoNotApplicableAnswer.No,
				"Does not meet",
				"Evidence");

			var response = await client.PutAsJsonAsync("/significant-change/99999/SpecialSchoolSatelliteConsiderations", request);

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
