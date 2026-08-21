using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class SignificantChangeProjectRepository(AcademisationContext context) : GenericRepository<SignificantChangeProject>(context), ISignificantChangeProjectRepository
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;

		public async Task<(IEnumerable<SignificantChangeProject> projects, int totalCount)>
			SearchSignificantChangeProjects(int page, int count, string? keyword, List<string>? status,
				List<string>? assignee, List<byte>? tier, List<string>? route, CancellationToken cancellationToken)
		{
			IQueryable<SignificantChangeProject> queryable = dbSet;

			queryable = FilterByStatus(status, queryable);
			queryable = FilterByKeyword(keyword, queryable);
			queryable = FilterByAssignee(assignee, queryable);
			queryable = FilterByTier(tier, queryable);
			queryable = FilterByRoute(route, queryable);

			int totalProjects = await queryable.CountAsync(cancellationToken);
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count)
				.ToListAsync(cancellationToken);

			return (projects, totalProjects);
		}

		private static IQueryable<SignificantChangeProject> FilterByRoute(List<string>? route, IQueryable<SignificantChangeProject> queryable)
		{
			if (route is null || route.Count == 0)
			{
				return queryable;
			}

			var lowerCaseRoutes = route.Select(x => x.ToLower()).ToArray();

			return queryable.Where(x => lowerCaseRoutes.Contains(x.TypeOfSignificantChange.ToLower()));
		}

		private static IQueryable<SignificantChangeProject> FilterByTier(List<byte>? tier, IQueryable<SignificantChangeProject> queryable)
		{
			if (tier is null || tier.Count == 0)
			{
				return queryable;
			}

			return queryable.Where(x => tier.Contains(x.Tier));
		}

		private static IQueryable<SignificantChangeProject> FilterByAssignee(List<string>? assignee,
			IQueryable<SignificantChangeProject> queryable)
		{
			if (assignee is null || assignee.Count == 0)
			{
				return queryable;
			}

			var lowerCaseAssignees = assignee.Select(x => x.ToLower()).ToArray();

			if (lowerCaseAssignees.Contains("not assigned"))
			{
				// Query by unassigned or assigned
				return queryable.Where(p =>
					(!string.IsNullOrEmpty(p.AssignedUserFullName) &&
					 lowerCaseAssignees.Contains(p.AssignedUserFullName.ToLower()))
					|| string.IsNullOrEmpty(p.AssignedUserFullName));
			}

			// Query by assigned only
			return queryable.Where(p =>
				!string.IsNullOrEmpty(p.AssignedUserFullName) &&
				lowerCaseAssignees.Contains(p.AssignedUserFullName.ToLower()));

		}

		private static IQueryable<SignificantChangeProject> FilterByKeyword(string? keyword, IQueryable<SignificantChangeProject> queryable)
		{
			if (string.IsNullOrWhiteSpace(keyword))
			{
				return queryable;
			}

			return queryable.Where(p =>
				EF.Functions.Like(p.SchoolName, $"%{keyword}%") ||
				EF.Functions.Like(p.TrustName, $"%{keyword}%") ||
				EF.Functions.Like(p.Urn.ToString(), $"%{keyword}%") ||
				EF.Functions.Like(p.TrustUkprn, $"%{keyword}%"));
		}

		private static IQueryable<SignificantChangeProject> FilterByStatus(List<string>? status, IQueryable<SignificantChangeProject> queryable)
		{
			if (status is null || status.Count == 0)
			{
				return queryable;
			}

			//convert List<string> to List<SignificantChangeStatus> and filter by status
			var significantChangeStatuses = status.Select(s => Enum.Parse<SignificantChangeStatus>(s, true)).ToList();


			return queryable.Where(p =>  significantChangeStatuses.Contains(p.Status));
		}

		public async Task<SignificantChangeProject?> GetSignificantChangeProjectById(int id, CancellationToken cancellationToken)
		{
			return await dbSet.SingleOrDefaultAsync(project => project.Id == id, cancellationToken);
		}

		public async Task<SignificantChangeFilterParameters> GetFilterParameters(CancellationToken cancellationToken)
		{
			List<string> assignedUsers = (await dbSet
				.AsNoTracking()
				.Select(project => project.AssignedUserFullName)
				.Where(fullName => !string.IsNullOrEmpty(fullName))
				.Distinct()
				.OrderBy(fullName => fullName)
				.ToListAsync(cancellationToken))!;

			List<string> routes = await dbSet
				.AsNoTracking()
				.Select(project => project.TypeOfSignificantChange)
				.Where(route => !string.IsNullOrEmpty(route))
				.Distinct()
				.OrderBy(route => route)
				.ToListAsync(cancellationToken);

			return new SignificantChangeFilterParameters
			{
				Statuses = Enum.GetValues<SignificantChangeStatus>()
					.Select(status => new FilterValueDisplay(status.ToString(), status.ToDisplayName()))
					.ToList(),

				Tiers = SignificantChangeTiers.All
					.Select(tier => new FilterValueDisplay(tier.ToString(), $"Tier {tier}"))
					.ToList(),

				AssignedUsers = assignedUsers
					.Select(fullName => new FilterValueDisplay(fullName, fullName))
					.ToList(),

				Routes = routes
					.Select(route => new FilterValueDisplay(route, route))
					.ToList()
			};
		}
	}
}
