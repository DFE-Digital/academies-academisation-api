using AutoFixture; 
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.SubcutaneousTest.Utils; 
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
			var command = new CreateProjectGroupCommand(Fixture.Create<string>()[..15], Fixture.Create<string>()[..7], Fixture.Create<string>()[..10], [], []);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("project-group/create-project-group",command, CancellationToken);

			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var response = await httpResponseMessage.ConvertResponseToTypeAsync<ProjectGroupResponseModel>();
			Assert.Equal(response.TrustName, command.TrustName);
			Assert.NotEmpty(response.ReferenceNumber!);
			Assert.NotEqual(response.Id, notExpectedId);
			Assert.Equal(response.Projects.Count, command.ConversionProjectIds.Count);
			Assert.Equal(response.Transfers?.Count, command.TransferProjectIds?.Count);
			Assert.Equal(response.TrustReferenceNumber, command.TrustReferenceNumber);
		}

		[Fact]
		public async Task CreateProjectGroupWithoutConversionAndTransfer_ShouldCreateSuccessfully()
		{
			// Arrange
			var notExpectedId = 0;
			var command = new CreateProjectGroupCommand(Fixture.Create<string>()[..15], Fixture.Create<string>()[..7], Fixture.Create<string>()[..10], [], []);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("project-group/create-project-group", command, CancellationToken);

			//Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var response = await httpResponseMessage.ConvertResponseToTypeAsync<ProjectGroupResponseModel>();
			Assert.Equal(response.TrustName, command.TrustName);
			Assert.NotEmpty(response.ReferenceNumber!);
			Assert.NotEqual(response.Id, notExpectedId);
			Assert.Equal(response.Projects.Count, command.ConversionProjectIds.Count);
			Assert.Equal(response.Transfers?.Count, command.TransferProjectIds?.Count);
			Assert.Equal(response.TrustReferenceNumber, command.TrustReferenceNumber);
			var projectGroup = await _context.ProjectGroups.FirstOrDefaultAsync(x => x.ReferenceNumber == response.ReferenceNumber);
			Assert.NotNull(projectGroup);
		}
		
		[Fact]
		public async Task CreateProjectGroupWithConversionAndTransfer_ShouldCreateSuccessfully()
		{
			// Arrange
			var conversionProject = (await CreateConversionProjects()).First();
			var trustUkprn = Fixture.Create<string>()[..10];
			var transferProject = (await CreateTransferProjects(trustUkprn, trustUkprn)).First(); 
			var command = new CreateProjectGroupCommand(Fixture.Create<string>()[..15], Fixture.Create<string>()[..7], trustUkprn, [conversionProject.Id], [transferProject.Id]);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("project-group/create-project-group", command, CancellationToken);

			//Assert 
			await VerifyProjectGroupAsync(httpResponseMessage, trustUkprn); 
		}

		[Fact]
		public async Task SetProjectGroupWithConversionAndTransfer_ShouldCreateSuccessfully()
		{
			// Arrange
			var projectGroup = await CreateProjectGroup();
			var conversionProject = (await CreateConversionProjects()).First();
			var transferProject = (await CreateTransferProjects(projectGroup.TrustUkprn, Fixture.Create<string>()[..7])).First();
			var command = new SetProjectGroupCommand([conversionProject.Id], [transferProject.Id]);

			// Action
			var httpResponseMessage = await _client.PutAsJsonAsync($"project-group/{projectGroup.ReferenceNumber}/set-project-group", command, CancellationToken);

			// Assert
			await VerifyProjectGroupAsync(httpResponseMessage, projectGroup.TrustUkprn);
		}

		[Fact]
		public async Task AssignProjectGroupWithUser_ShouldCreateSuccessfully()
		{
			// Arrange
			var projectGroup = await CreateProjectGroup();
			var conversionProjects = await CreateConversionProjects(1, projectGroup.Id);
			var command = new SetProjectGroupAssignUserCommand(Guid.NewGuid(), "Firstname", "first@email.com");

			// Action
			var httpResponseMessage = await _client.PutAsJsonAsync($"project-group/{projectGroup.ReferenceNumber}/assign-project-group-user", command, CancellationToken);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var dbProjectGroup = await _context.ProjectGroups.AsNoTracking().FirstAsync(x => x.TrustUkprn == projectGroup.TrustUkprn);
			Assert.NotNull(dbProjectGroup);
			Assert.Equal(dbProjectGroup.AssignedUser!.FullName, command.FullName);
			Assert.Equal(dbProjectGroup.AssignedUser!.EmailAddress, command.EmailAddress); 
		}

		[Fact]
		public async Task SetProjectGroupByRemovingOneConversionAndTransfer_ShouldCreateSuccessfully()
		{
			// Arrange
			var projectGroup = await CreateProjectGroup();
			var conversionProjects = await CreateConversionProjects(2, projectGroup.Id);
			var transferProjects = await CreateTransferProjects(projectGroup.TrustUkprn, Fixture.Create<string>()[..7], 2, projectGroup.Id);
			var command = new SetProjectGroupCommand(conversionProjects.Skip(1).Select(x=>x.Id).ToList(), transferProjects.Skip(1).Select(x => x.Id).ToList());

			// Action
			var httpResponseMessage = await _client.PutAsJsonAsync($"project-group/{projectGroup.ReferenceNumber}/set-project-group", command, CancellationToken);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode); 
			var addedConversionProject = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == command.ConversionProjectIds.First());
			Assert.NotNull(addedConversionProject);
			Assert.Equal(addedConversionProject.ProjectGroupId, projectGroup.Id);

			var addedTransferProject = await _context.TransferProjects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == command.TransferProjectIds!.First());
			Assert.NotNull(addedTransferProject);
			Assert.Equal(addedTransferProject.ProjectGroupId, projectGroup.Id);

			var removedConversionProject = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == conversionProjects.First().Id);
			Assert.NotNull(removedConversionProject);
			Assert.Null(removedConversionProject.ProjectGroupId);

			var removedTransferProject = await _context.TransferProjects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == transferProjects.First().Id);
			Assert.NotNull(removedTransferProject);
			Assert.Null(removedTransferProject.ProjectGroupId);
		}

		private async Task<List<Project>> CreateConversionProjects(int count = 1, int? projectGroupId = null)
		{
			var projects = new List<Project>();
			for (int i = 0; i < count; i++) 
			{
				var project = Fixture.Build<Project>()
				.Without(x => x.Id)
				.Create();

				project.SetProjectGroupId(projectGroupId);

				_context.Projects.Add(project);
				await _context.SaveChangesAsync();
				projects.Add(project);
			}
			return projects;
		}

		private async Task<List<ITransferProject>> CreateTransferProjects(string incomingTrustUkprn, string outgoingTrustUkprn, int count = 1, int? projectGroupId = null)
		{
			var projects = new List<ITransferProject>();
			for (int i = 0; i < count; i++)
			{
				outgoingTrustUkprn = $"{outgoingTrustUkprn}{i}";
				var transferAcademy = new TransferringAcademy(incomingTrustUkprn, "in trust", outgoingTrustUkprn, "region", "local authority");
				var transferringAcademies = new List<TransferringAcademy>() { transferAcademy };

				var transferProject = TransferProject.Create(outgoingTrustUkprn, "out trust", transferringAcademies, false, DateTime.Now);
				transferProject.SetProjectGroupId(projectGroupId);
				_context.TransferProjects.Add(transferProject);
				await _context.SaveChangesAsync();

				projects.Add(await _context.TransferProjects.AsNoTracking().FirstAsync(x =>  x.OutgoingTrustUkprn == outgoingTrustUkprn));
			}
			return projects;
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

		private async Task VerifyProjectGroupAsync(HttpResponseMessage httpResponseMessage, string trustUkprn)
		{
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var projectGroup = await _context.ProjectGroups.FirstAsync(x => x.TrustUkprn == trustUkprn);
			var dbConversionProject = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.ProjectGroupId == projectGroup.Id);
			Assert.NotNull(dbConversionProject);
			Assert.Equal(dbConversionProject.ProjectGroupId, projectGroup.Id);

			var dbTransferProject = await _context.TransferProjects.AsNoTracking().FirstOrDefaultAsync(x => x.ProjectGroupId == projectGroup.Id);
			Assert.NotNull(dbTransferProject);
			Assert.Equal(dbTransferProject.ProjectGroupId, projectGroup.Id);
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
	}
}
