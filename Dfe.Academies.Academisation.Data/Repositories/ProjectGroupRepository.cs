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

		public async Task<(IEnumerable<IProjectGroup>, int totalcount)> SearchProjectGroups(int page, int count, string? referenceNumber, IEnumerable<string?>? trustReferences, CancellationToken cancellationToken)
		{
			IQueryable<ProjectGroup> queryable = DefaultIncludes();
			queryable = FilterByReferenceNumber(referenceNumber, queryable);
			queryable = FilterByTrust(trustReferences, queryable);

			var totalProjectGroups = queryable.Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync(cancellationToken);

			return (projects, totalProjectGroups);
		}

		private static IQueryable<ProjectGroup> FilterByReferenceNumber(string? referenceNumber, IQueryable<ProjectGroup> queryable)
		{
			if (!referenceNumber.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => p.ReferenceNumber == referenceNumber);
			}

			return queryable;
		}

		private  IQueryable<ProjectGroup> FilterByTrust(IEnumerable<string> trustReferences, IQueryable<ProjectGroup> queryable)
		{
			if (trustReferences.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => trustReferences.Contains(p.TrustReference));
			}

			return queryable;
		}

		public async Task<List<IProjectGroup>> GetByIds(IEnumerable<int?> projectGroupIds, CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => projectGroupIds.Contains(x.Id)).Cast<IProjectGroup>().ToListAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
