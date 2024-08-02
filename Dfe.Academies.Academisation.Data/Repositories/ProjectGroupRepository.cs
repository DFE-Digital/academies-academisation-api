using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class ProjectGroupRepository : GenericRepository<ProjectGroup>, IProjectGroupRepository
	{
		private readonly AcademisationContext _context;
		public ProjectGroupRepository(AcademisationContext context) : base(context) 
			=> _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;

		private IQueryable<ProjectGroup> DefaultIncludes()
		{
			var x = dbSet
				.Include(x => x.AssignedUser)
				.AsQueryable();

			return x;
		}

		public AcademisationContext Context() => _context;

		public async Task<ProjectGroup?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken) 
			=> await DefaultIncludes().SingleOrDefaultAsync(x => x.ReferenceNumber == referenceNumber, cancellationToken);

		public async Task<IEnumerable<IProjectGroup>> SearchProjectGroups(string searchTerm, CancellationToken cancellationToken)
		{
			IQueryable<ProjectGroup> queryable = DefaultIncludes();
			queryable = FilterBySearchTerm(searchTerm, queryable);

			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.ToListAsync(cancellationToken);

			return projects;
		}

		private static IQueryable<ProjectGroup> FilterBySearchTerm(string searchTerm, IQueryable<ProjectGroup> queryable)
		{
			if (!searchTerm.IsNullOrEmpty())
			{
				searchTerm = searchTerm.ToLower();
				queryable = queryable.Where(p => (p.ReferenceNumber != null && p.ReferenceNumber.ToLower().Contains(searchTerm)) || 
				p.TrustReference.ToLower().Contains(searchTerm) ||
				p.TrustUkprn.ToLower().Contains(searchTerm) ||
				p.TrustName.ToLower().Contains(searchTerm));
			}

			return queryable;
		}

		public async Task<IEnumerable<IProjectGroup>> GetByIds(IEnumerable<int?> projectGroupIds, CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => projectGroupIds.Contains(x.Id)).Cast<IProjectGroup>().ToListAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
