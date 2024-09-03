using System.Net;
using AutoFixture;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate; 
using Dfe.Academies.Academisation.IDomain.UserRoleAggregate;
using Dfe.Academies.Academisation.Service.Commands.UserRole;
using Dfe.Academies.Academisation.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Dfe.Academies.Academisation.SubcutaneousTest.Utils;
using Dfe.Academies.Academisation.IService.ServiceModels.UserRole;

namespace Dfe.Academies.Academisation.SubcutaneousTest.UserRole
{
	public class UserRoleTests : ApiIntegrationTestBase
	{
		private readonly HttpClient _client;
		private readonly AcademisationContext _context;

		public UserRoleTests()
		{
			_client = CreateClient();
			_context = GetDBContext();
		}

		[Fact]
		public async Task CreateUserRole_ShouldCreateSuccessfully()
		{
			// Arrange 
			var command = new CreateUserRoleCommand(Guid.NewGuid(),
				Fixture.Create<string>()[..7], $"{Fixture.Create<string>()[..5]}@email.com", RoleId.Standard, true);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("user-role/create-user-role", command, CancellationToken);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var userRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == command.EmailAddress); 
			Assert.NotNull(userRole);
			Assert.Equal(userRole.RoleId, command.RoleId);
			Assert.Equal(userRole.IsEnabled, command.IsEnabled);
			Assert.Equal(userRole.AssignedUser!.FullName, command.FullName);
			Assert.Equal(userRole.AssignedUser!.Id, command.UserId);
		}

		[Fact]
		public async Task CreateUserRole_ShouldReturnValidationError()
		{
			// Arrange 
			var errorCount = 2;
			var errorTitle = "One or more validation errors occurred.";
			var command = new CreateUserRoleCommand(Guid.NewGuid(), string.Empty, $"{Fixture.Create<string>()[..5]}", RoleId.Standard, true);

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("user-role/create-user-role", command, CancellationToken);

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			var userRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == command.EmailAddress);
			Assert.Null(userRole);
			var errorModel = await httpResponseMessage.ConvertResponseToTypeAsync<ValidationErrorModel>();
			Assert.NotNull(errorModel);
			Assert.Equal(errorModel.Status, HttpStatusCode.BadRequest.GetHashCode());
			Assert.Equal(errorModel.Title, errorTitle);
			Assert.Equal(errorModel.Errors.DomainValidations.Count, errorCount);
		}

		[Fact]
		public async Task SetUserRole_ShouldCreateSuccessfully()
		{
			// Arrange 
			var userRole = await CreateUserRole();
			var command = new SetUserRoleCommand(RoleId.Standard, true);

			// Action
			var httpResponseMessage = await _client.PutAsJsonAsync($"user-role/{userRole.AssignedUser!.EmailAddress}/assign-user-role", command, CancellationToken);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			var dbUserRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == userRole.AssignedUser!.EmailAddress);
			Assert.NotNull(dbUserRole);
			Assert.Equal(dbUserRole!.RoleId, command.RoleId);
			Assert.Equal(dbUserRole.IsEnabled, command.IsEnabled);
		}

		[Fact]
		public async Task SetUserRole_ShouldReturnValidationError()
		{
			// Arrange 
			var userRole = await CreateUserRole();
			var errorCount = 1;
			var email = $"{Fixture.Create<string>()[..5]}";
			var errorTitle = "One or more validation errors occurred."; 
			var command = new SetUserRoleCommand(RoleId.Standard, true);

			// Action
			var httpResponseMessage = await _client.PutAsJsonAsync($"user-role/{email}/assign-user-role", command, CancellationToken);

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			var dbUserRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == userRole.AssignedUser!.EmailAddress);
			Assert.NotNull(dbUserRole);
			Assert.Equal(dbUserRole.RoleId, userRole.RoleId);
			Assert.Equal(dbUserRole.IsEnabled, userRole.IsEnabled);
			var errorModel = await httpResponseMessage.ConvertResponseToTypeAsync<ValidationErrorModel>();
			Assert.NotNull(errorModel);
			Assert.Equal(errorModel.Status, HttpStatusCode.BadRequest.GetHashCode());
			Assert.Equal(errorModel.Title, errorTitle);
			Assert.Equal(errorModel.Errors.DomainValidations.Count, errorCount);
		}

		[Fact]
		public async Task SetUserRole_ShouldReturnNotFound()
		{
			// Arrange 
			var userRole = await CreateUserRole();
			var email = $"{Fixture.Create<string>()[..5]}@email.com";
			var errorTitle = "Not Found";
			var command = new SetUserRoleCommand(RoleId.Standard, true);

			// Action
			var httpResponseMessage = await _client.PutAsJsonAsync($"user-role/{email}/assign-user-role", command, CancellationToken);

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			var dbUserRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == userRole.AssignedUser!.EmailAddress);
			Assert.NotNull(dbUserRole);
			Assert.Equal(dbUserRole.RoleId, userRole.RoleId);
			Assert.Equal(dbUserRole.IsEnabled, userRole.IsEnabled);
			var errorModel = await httpResponseMessage.ConvertResponseToTypeAsync<ValidationErrorModel>();
			Assert.NotNull(errorModel);
			Assert.Equal(errorModel.Status, HttpStatusCode.NotFound.GetHashCode());
			Assert.Equal(errorModel.Title, errorTitle);
		}

		[Fact]
		public async Task GetUserRoleCapabilities_ShouldReturnCapabilities()
		{
			// Arrange 
			var userRole = await CreateUserRole();
			var capabilities = Roles.GetRoleCapabilities(userRole.RoleId);

			// Action
			var httpResponseMessage = await _client.GetAsync($"user-role/{userRole.AssignedUser!.EmailAddress}", CancellationToken);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), HttpStatusCode.OK.GetHashCode());
			var roleCapabilitiesModel = await httpResponseMessage.ConvertResponseToEnumTypeAsync<RoleCapabilitiesModel>();

			for (int i = 0; i < capabilities.Count; i++)
			{
				Assert.Equal(capabilities[i], roleCapabilitiesModel.Capabilities[i]);
			}
		}

		[Fact]
		public async Task GetUserRoleCapabilities_ShouldReturnNotFound()
		{
			// Arrange
			var email = $"{Fixture.Create<string>()}@notfound.com";
			var errorMessage = $"User role capabilities with {email} email not found.";

			// Action
			var httpResponseMessage = await _client.GetAsync($"user-role/{email}", CancellationToken);

			// Assert
			Assert.False(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), HttpStatusCode.NotFound.GetHashCode());
			var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();

			Assert.Equal(responseMessage, errorMessage);
		}

		[Theory]
		[InlineData("","first.lastname@email.com", null)]
		[InlineData("Full Name", "", null)]
		[InlineData("", "", RoleId.Standard)]
		public async Task SearchUsers_ShouldReturnUsers(string fullName, string email, RoleId? roleId)
		{
			// Arrange  
			var searchItem = !string.IsNullOrEmpty(fullName) ? fullName : (!string.IsNullOrEmpty(email) ? email : null);
			var searchModel = new UserRoleSearchModel(1, 1, searchItem, roleId);
			var userRole = await CreateUserRole();

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("/user-role/search-users", searchModel, CancellationToken);

			// Assert
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), HttpStatusCode.OK.GetHashCode());
			var usersModel = await httpResponseMessage.ConvertResponseToPagedDataResponseAsync<UserRoleModel>();

			usersModel.Data.ToList().ForEach(user =>
			{
				Assert.Equal(user.Email, userRole.AssignedUser!.EmailAddress);
				Assert.Equal(user.FullName, userRole.AssignedUser!.FullName);
				Assert.Equal(user.RoleId, userRole.RoleId);
			});
		}

		private async Task<IUserRole> CreateUserRole(string fullName = "Full Name", string emailAddress = "first.lastname@email.com")
		{
			var userRole = new Domain.UserRoleAggregate.UserRole(RoleId.Standard, true, DateTime.Now);
			userRole.SetAssignedUser(Guid.NewGuid(), fullName, emailAddress);

			_context.UserRoles.Add(userRole);
			await _context.SaveChangesAsync(CancellationToken);

			return userRole;
		}
	}
}
