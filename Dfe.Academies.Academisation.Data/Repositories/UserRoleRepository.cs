using Dfe.Academies.Academisation.Domain.Core.UserRoleAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.UserRoleAggregate;
using Dfe.Academies.Academisation.IDomain.UserRoleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class UserRoleRepository(AcademisationContext context, IRoleInfo roleInfo) : GenericRepository<UserRole>(context), IUserRoleRepository
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));
		private readonly IRoleInfo _roleInfo = roleInfo ?? throw new ArgumentNullException(nameof(roleInfo));	

		public IUnitOfWork UnitOfWork => _context;

		public async Task<IEnumerable<IUserRole>> SearchUsersAsync(string? searchTerm, RoleId? roleId, CancellationToken cancellationToken)
		{
			var queryable = DefaultIncludes();
			queryable = FilterBySearchTerm(searchTerm, queryable);
			queryable = FilterByRoleId(roleId, queryable);

			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.ToListAsync(cancellationToken);

			return projects;
		}
		public async Task<IUserRole?> GetUserWithRoleAsync(string email, CancellationToken cancellationToken) 
			=> await DefaultIncludes().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == email, cancellationToken);

		public async Task<List<RoleCapability>> GetUserRoleCapabilitiesAsync(string email, CancellationToken cancellationToken)
		{
			var userRole = await DefaultIncludes().FirstOrDefaultAsync(x => x.AssignedUser!.EmailAddress == email, cancellationToken);
			return userRole == null ? Roles.GetRoleCapabilities(RoleId.Standard) : Roles.GetRoleCapabilities(_roleInfo.GetEnum(userRole.RoleId));
		}
		private static IQueryable<UserRole> FilterBySearchTerm(string? searchTerm, IQueryable<UserRole> queryable)
		{
			if (!searchTerm.IsNullOrEmpty())
			{
				searchTerm = searchTerm!.ToLower();
				queryable = queryable.Where(p => (p.AssignedUser != null && p.AssignedUser.EmailAddress.Contains(searchTerm)) ||
				p.AssignedUser!.FullName.Contains(searchTerm));
			}

			return queryable;
		}

		private IQueryable<UserRole> FilterByRoleId(RoleId? roleId, IQueryable<UserRole> queryable)
		{
			if (roleId != null)
			{
				var id = _roleInfo.GetId(roleId.Value);
				queryable = queryable.Where(p => p.RoleId == id);
			}

			return queryable;
		}

		private IQueryable<UserRole> DefaultIncludes()
		{
			var x = dbSet
				.Include(x => x.AssignedUser)
				.AsQueryable();

			return x;
		}
	}
}
