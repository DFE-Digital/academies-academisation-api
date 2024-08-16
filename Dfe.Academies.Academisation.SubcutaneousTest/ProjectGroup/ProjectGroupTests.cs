using System.Web.Http.Results;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.SubcutaneousTest.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			var notExpectedId = 0;
			var command = new CreateProjectGroupCommand(Fixture.Create<string>()[..15], Fixture.Create<string>()[..7], Fixture.Create<string>()[..10], []);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("project-group/create-project-group",command, CancellationToken);

			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var response = await httpResponseMessage.ConvertResponseToTypeAsync<ProjectGroupResponseModel>();
			Assert.Null(response.TrustName);
			Assert.NotEmpty(response.ReferenceNumber!);
			Assert.NotEqual(response.Id, notExpectedId);
			Assert.Equal(response.TrustReferenceNumber, command.TrustReferenceNumber);
		}

		[Fact]
		public async Task DeleteProjectGroup_ShouldDeleteSuccessfully()
		{
			// Arrange
			var projectGroup = await CreateProjectGroup();

			// Action
			var httpResponseMessage = await _client.DeleteAsync($"project-group/{projectGroup.ReferenceNumber}", CancellationToken);

			Assert.True(httpResponseMessage.IsSuccessStatusCode); 
			var dbProjectGroup = await _context.ProjectGroups.AsNoTracking().FirstOrDefaultAsync(x => x.ReferenceNumber == projectGroup.ReferenceNumber);
			Assert.Null(dbProjectGroup);
		}

		[Fact]
		public async Task DeleteProjectGroup_ShouldReturnNotFoundOnNotFindingGroup()
		{
			// Arrange
			var referenceNumber = Fixture.Create<string>()[..10];

			// Action
			var httpResponseMessage = await _client.DeleteAsync($"project-group/{referenceNumber}", CancellationToken);
		
			// Assert 
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage!.StatusCode.GetHashCode(), System.Net.HttpStatusCode.NotFound.GetHashCode());
		}


		private async Task<Domain.ProjectGroupsAggregate.ProjectGroup> CreateProjectGroup(bool assignUser = false)
		{
			var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(Fixture.Create<string>()[..7],
				Fixture.Create<string>()[..7],
				Fixture.Create<string>()[..7],
				DateTime.Now);
			if (assignUser)
			{
				projectGroup.SetAssignedUser(Guid.NewGuid(), "Full Name", "First@email.com");
			}
			projectGroup.SetProjectReference(1);
			_context.ProjectGroups.Add(projectGroup);
			await _context.SaveChangesAsync();
			return await _context.ProjectGroups.AsNoTracking().FirstAsync(x => x.TrustUkprn == projectGroup.TrustUkprn);
		}
	}
}
