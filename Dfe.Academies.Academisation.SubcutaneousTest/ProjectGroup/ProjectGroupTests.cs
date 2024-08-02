using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.SubcutaneousTest.Utils;

namespace Dfe.Academies.Academisation.SubcutaneousTest.ProjectGroup
{
	public class ProjectGroupTests : ApiIntegrationTestBase
	{
		private readonly HttpClient _client;
		private readonly AcademisationContext _context;

		public ProjectGroupTests()
		{
			_client = CreateClient();
			_context = GetDBContext();
		}

		[Fact]
		public async Task CreateProjectGroup_ShouldCreateSuccessfully()
		{
			// Arrange
			var command = new CreateProjectGroupCommand(Fixture.Create<string>()[..15], Fixture.Create<string>()[..7], Fixture.Create<string>()[..10], []);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("project-group/create-project-group",command, CancellationToken);

			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var response = await httpResponseMessage.ConvertResponseToTypeAsync<ProjectGroupResponseModel>();
			Assert.Equal(response.TrustName, command.TrustName);
			Assert.NotEmpty(response.ReferenceNumber!);
			Assert.Equal(response.TrustReferenceNumber, command.TrustReferenceNumber);
		}
	}
}
