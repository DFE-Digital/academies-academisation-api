using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
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

		public async Task<(IEnumerable<IProjectGroup>, int totalcount)> SearchProjectGroups(int page, int count, string? urn, string? trustUrn, string? trustName, string? academyName, string? academyUkprn, string? companiesHouseNo, CancellationToken cancellationToken)
		{
			IQueryable<ProjectGroup> queryable = DefaultIncludes();
			queryable = FilterByUrn(urn, queryable);
			queryable = FilterByTrust(trustUrn, trustName, queryable);
			queryable = FilterByAcademy(academyUkprn, academyName, queryable);
			queryable = FilterByCompaniesHouseNo(companiesHouseNo, queryable);

			var totalProjectGroups = queryable.Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync(cancellationToken);

			return (projects, totalProjectGroups);
		}

		private IQueryable<ProjectGroup> FilterByCompaniesHouseNo(string? companiesHouseNo, IQueryable<ProjectGroup> queryable)
		{
			if (!companiesHouseNo.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => p.ReferenceNumber == companiesHouseNo);
			}

			return queryable;
		}

		private IQueryable<ProjectGroup> FilterByAcademy(string? academyUkprn, string? academyName, IQueryable<ProjectGroup> queryable)
		{
			if (!academyUkprn.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => p.ReferenceNumber == academyUkprn);
			}

			if (!academyName.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => p.ReferenceNumber == academyName);
			}

			return queryable;
		}

		private static IQueryable<ProjectGroup> FilterByUrn(string? urn, IQueryable<ProjectGroup> queryable)
		{
			if (!urn.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => p.ReferenceNumber == urn);
			}

			return queryable;
		}
		private  IQueryable<ProjectGroup> FilterByTrust(string? trustUrn, string? trustName, IQueryable<ProjectGroup> queryable)
		{
			if (!trustUrn.IsNullOrEmpty())
			{
				queryable = queryable.Where(p => p.TrustReference == trustUrn);
			}
			if (!trustName.IsNullOrEmpty())
			{
				trustUrn = "todo";
				queryable = queryable.Where(p => p.TrustReference == trustUrn);
			}

			return queryable;
		}
	}
}
