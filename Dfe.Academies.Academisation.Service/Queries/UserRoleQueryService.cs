using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.Domain.UserRoleAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.UserRole;
using Dfe.Academies.Academisation.Service.Factories; 

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class UserRoleQueryService(IUserRoleRepository userRoleRepository): IUserRoleQueryService
	{
		public async Task<RoleCapabilitiesModel> GetUserRoleCapabilitiesAsync(string email, CancellationToken cancellationToken)
			=> new RoleCapabilitiesModel(await userRoleRepository.GetUserRoleCapabilitiesAsync(email, cancellationToken));

		public async Task<PagedDataResponse<UserRoleModel>> SearchUsersAsync(string? searchTerm, RoleId? roleId, int page, int count, CancellationToken cancellationToken)
		{
			var userRoles = await userRoleRepository.SearchUsersAsync(searchTerm, roleId, cancellationToken);

			var pageResponse = PagingResponseFactory.Create("user-role/users", page, count, userRoles.Count(), []);

			var data =  userRoles.Select(userRole 
				=> new UserRoleModel(GetRoleIdEnum(userRole.RoleId), userRole.AssignedUser!.FullName, userRole.AssignedUser!.EmailAddress)).ToList();

			return new PagedDataResponse<UserRoleModel>(data, pageResponse);
		}
		private static RoleId GetRoleIdEnum(string roleId)
		{
			return roleId == RoleId.Manager.GetStringValue()
				? RoleId.Manager : roleId == RoleId.SuperAdmin.GetStringValue()
				? RoleId.SuperAdmin : RoleId.Standard;
		}
	} 
}
