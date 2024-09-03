using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.UserRoleAggregate;

namespace Dfe.Academies.Academisation.Domain.UserRoleAggregate
{
	public interface IUserRoleRepository : IRepository<UserRole>, IGenericRepository<UserRole>
	{
		Task<List<RoleCapability>> GetUserRoleCapabilitiesAsync(string email, CancellationToken cancellationToken);
		Task<IUserRole?> GetUserWithRoleAsync(string email, CancellationToken cancellationToken);
		Task<IEnumerable<IUserRole>> SearchUsersAsync(string? searchTerm, RoleId? roleId, CancellationToken cancellationToken);
	}
}
