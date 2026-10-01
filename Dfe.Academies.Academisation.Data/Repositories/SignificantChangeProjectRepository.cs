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
			SearchSignificantChangeProjects(SignificantChangeProjectSearchOptions arguments, CancellationToken cancellationToken)
		{
			IQueryable<SignificantChangeProject> queryable = dbSet;

			queryable = FilterByStatus(arguments.Status, queryable);
			queryable = FilterByKeyword(arguments.Keyword, queryable);
			queryable = FilterByAssignee(arguments.Assignee, queryable);
			queryable = FilterByTier(arguments.Tier, queryable);
			queryable = FilterByRoute(arguments.Route, queryable);
			queryable = FilterByLocalAuthority(arguments.LocalAuthorities, queryable);
			queryable = FilterByRegion(arguments.Regions, queryable);

			int totalProjects = await queryable.CountAsync(cancellationToken);
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((arguments.Page - 1) * arguments.Count)
				.Take(arguments.Count)
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

		private static IQueryable<SignificantChangeProject> FilterByLocalAuthority(List<string>? localAuthorities, IQueryable<SignificantChangeProject> queryable)
		{
			if (localAuthorities is null || localAuthorities.Count == 0)
			{
				return queryable;
			}

			string[] lowerCaseLocalAuthorities = [.. localAuthorities.Select(x => x.ToLower())];

			return queryable.Where(x => !string.IsNullOrEmpty(x.LocalAuthorityName) && lowerCaseLocalAuthorities.Contains(x.LocalAuthorityName.ToLower()));
		}

		private static IQueryable<SignificantChangeProject> FilterByRegion(List<string>? regions, IQueryable<SignificantChangeProject> queryable)
		{
			if (regions is null || regions.Count == 0)
			{
				return queryable;
			}

			string[] lowerCaseRegions = [.. regions.Select(x => x.ToLower())];

			return queryable.Where(x => !string.IsNullOrEmpty(x.RegionName) && lowerCaseRegions.Contains(x.RegionName.ToLower()));
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

			List<string> localAuthorities = await dbSet
				.AsNoTracking()
				.Select(project => project.LocalAuthorityName)
				.Where(localAuthority => !string.IsNullOrEmpty(localAuthority))
				.Select(localAuthority => localAuthority!)
				.Distinct()
				.OrderBy(localAuthority => localAuthority)
				.ToListAsync(cancellationToken);

			List<string> regions = await dbSet
				.AsNoTracking()
				.Select(project => project.RegionName)
				.Where(region => !string.IsNullOrEmpty(region))
				.Select(region => region!)
				.Distinct()
				.OrderBy(region => region)
				.ToListAsync(cancellationToken);

			return new SignificantChangeFilterParameters
			{
				Statuses = [.. Enum.GetValues<SignificantChangeStatus>().Select(status => new FilterValueDisplay(status.ToString(), status.ToDisplayName()))],
				Tiers = [.. SignificantChangeTiers.All.Select(tier => new FilterValueDisplay(tier.ToString(), tier.ToString()))],
				AssignedUsers = [.. assignedUsers.Select(fullName => new FilterValueDisplay(fullName, fullName))],
				Routes = [.. routes.Select(route => new FilterValueDisplay(route, route))],
				LocalAuthorities = [.. localAuthorities.Select(localAuthority => new FilterValueDisplay(localAuthority, localAuthority))],
				Regions = [.. regions.Select(region => new FilterValueDisplay(region, region))]
			};
		}

		public async Task<List<SignificantChangeProject>> GetProjectsToSendToCompleteAsync(CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(proj => !proj.ProjectSentToComplete && proj.ReadOnlyDate.HasValue).ToListAsync(cancellationToken);
		}
	}
}
