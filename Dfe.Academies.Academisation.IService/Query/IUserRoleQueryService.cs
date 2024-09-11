using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.UserRole; 

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IUserRoleQueryService
	{
		Task<RoleCapabilitiesModel> GetUserRoleCapabilitiesAsync(string email, CancellationToken cancellationToken);

		Task<PagedDataResponse<UserRoleModel>> SearchUsersAsync(string? searchTerm, RoleId? roleId, int page, int count, CancellationToken cancellationToken);
	}
}
