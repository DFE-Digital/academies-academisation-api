
using System.Globalization;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class ConversionProjectRepository : GenericRepository<Project>, IConversionProjectRepository
	{
		private readonly IMapper _mapper;
		private readonly AcademisationContext _context;

		public ConversionProjectRepository(AcademisationContext context, IMapper mapper) : base(context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper;
		}

		public IUnitOfWork UnitOfWork => _context;
		public async Task<(IEnumerable<IProject>, int)> SearchProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions = default, IEnumerable<string>? applicationReferences = default)
		{
			IQueryable<Project> queryable = dbSet;

			queryable = FilterByRegion(regions, queryable);
			queryable = FilterByStatus(states, queryable);
			queryable = FilterByUrn(urn, queryable);
			queryable = FilterBySchool(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);
			queryable = FilterByApplicationIds(applicationReferences, queryable);

			var totalProjects = queryable.Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync();

			return (projects, totalProjects);
		}

		public async Task<ProjectFilterParameters> GetFilterParameters()
		{
			var advisoryBoardDates = await dbSet
					.OrderByDescending(p => p.Details.HeadTeacherBoardDate)
					.AsNoTracking()
					.Where(p => p.Details.HeadTeacherBoardDate.HasValue)
					.Select(p => new { p.Details.HeadTeacherBoardDate.Value.Year, p.Details.HeadTeacherBoardDate.Value.Month })
					.Distinct()
					.ToListAsync()
					.ConfigureAwait(false);

			ProjectFilterParameters filterParameters = new ProjectFilterParameters
			{
				Statuses = (await dbSet
					.AsNoTracking()
					.Select(p => p.Details.ProjectStatus)
					.Distinct()
					.OrderBy(p => p)
					.ToListAsync())!,
				AssignedUsers = (await dbSet
					.OrderByDescending(p => p.Details.AssignedUser.FullName)
					.AsNoTracking()
					.Select(p => p.Details.AssignedUser.FullName)
					.Where(p => !string.IsNullOrEmpty(p))
					.Distinct()
					.ToListAsync())!,

				LocalAuthorities = (await dbSet
					.OrderByDescending(p => p.Details.LocalAuthority)
					.AsNoTracking()
					.Select(p => p.Details.LocalAuthority)
					.Where(p => !string.IsNullOrEmpty(p))
					.Distinct()
					.ToListAsync())!,

				AdvisoryBoardDates = advisoryBoardDates.Select(x => new DateTime(x.Year, x.Month, 1).ToString("MMM yy")).ToList()
			};

			return filterParameters;
		}

		private static IQueryable<Project> FilterFormAMAT(IQueryable<Project> queryable)
		{
			queryable = queryable.Where(p => p.FormAMatProjectId != null);

			return queryable;
		}

		private static IQueryable<Project> FilterGroupedProjects(IQueryable<Project> queryable)
		{
			queryable = queryable.Where(p => p.ProjectGroupId != null);

			return queryable;
		}

		public async Task<IEnumerable<IProject>?> GetIncompleteProjects()
		{
			var createdProjectState = await _context.Projects
				.Where(p => string.IsNullOrEmpty(p.Details.LocalAuthority) || string.IsNullOrEmpty(p.Details.Region) || string.IsNullOrEmpty(p.Details.SchoolPhase) || string.IsNullOrEmpty(p.Details.SchoolType))
				.ToListAsync();

			return createdProjectState;
		}

		private static IQueryable<Project> FilterByRegion(IEnumerable<string>? regions, IQueryable<Project> queryable)
		{

			if (regions != null && regions.Any())
			{
				var lowerCaseRegions = regions.Select(region => region.ToLower());
				queryable = queryable.Where(p =>
					!string.IsNullOrEmpty(p.Details.Region) && lowerCaseRegions.Contains(p.Details.Region.ToLower()));
			}

			return queryable;
		}

		private static IQueryable<Project> FilterByStatus(IEnumerable<string>? states, IQueryable<Project> queryable)
		{
			if (states != null && states!.Any())
			{
				queryable = queryable.Where(p => states.Contains(p.Details.ProjectStatus!.ToLower()));
			}

			return queryable;
		}
		private static IQueryable<Project> FilterByApplicationIds(IEnumerable<string>? applicationIds, IQueryable<Project> queryable)
		{
			if (applicationIds != null && applicationIds!.Any())
			{
				queryable = queryable.Where(p => applicationIds.Contains(p.Details.ApplicationReferenceNumber!));
			}

			return queryable;
		}

		private static IQueryable<Project> FilterByUrn(int? urn, IQueryable<Project> queryable)
		{
			if (urn.HasValue) queryable = queryable.Where(p => p.Details.Urn == urn);

			return queryable;
		}

		private static IQueryable<Project> FilterBySchool(string? title, IQueryable<Project> queryable)
		{
			if (!string.IsNullOrWhiteSpace(title)) queryable = queryable.Where(p => p.Details.SchoolName!.ToLower().Contains(title!.ToLower()));

			return queryable;
		}

		private static IQueryable<Project> FilterByKeyword(string? title, IQueryable<Project> queryable)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{

				queryable = queryable.Where(p => p.Details.SchoolName!.ToLower().Contains(title!.ToLower()) ||
				p.Details.NameOfTrust!.ToLower().Contains(title!.ToLower()) ||
				p.Details.Urn.ToString().ToLower().Contains(title!.ToLower())
				);
			}

			return queryable;
		}

		private static IQueryable<Project> FilterByDeliveryOfficer(IEnumerable<string>? deliveryOfficers, IQueryable<Project> queryable)
		{
			if (deliveryOfficers != null && deliveryOfficers.Any())
			{
				var lowerCaseDeliveryOfficers = deliveryOfficers.Select(officer => officer.ToLower());

				if (lowerCaseDeliveryOfficers.Contains("not assigned"))
				{
					// Query by unassigned or assigned delivery officer
					queryable = queryable.Where(p =>
								(!string.IsNullOrEmpty(p.Details.AssignedUser.FullName) && lowerCaseDeliveryOfficers.Contains(p.Details.AssignedUser.FullName.ToLower()))
								|| string.IsNullOrEmpty(p.Details.AssignedUser.FullName));
				}
				else
				{
					// Query by assigned delivery officer only
					queryable = queryable.Where(p =>
						!string.IsNullOrEmpty(p.Details.AssignedUser.FullName) && lowerCaseDeliveryOfficers.Contains(p.Details.AssignedUser.FullName.ToLower()));
				}
			}

			return queryable;
		}

		public async Task<IProject?> GetConversionProject(int id, CancellationToken cancellationToken)
		{
			return await DefaultIncludes().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
		}

		private IQueryable<Project> DefaultIncludes()
		{
			var x = _context.Projects
				.Include(x => x.Notes)
				.Include(x => x.SchoolImprovementPlans)
				.AsQueryable();

			return x;
		}

		public async Task<(IEnumerable<IProject> projects, int totalCount)> SearchProjectsV2(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count)
		{
			IQueryable<Project> queryable = dbSet;

			queryable = FilterByRegion(regions, queryable);
			queryable = FilterByStatus(states, queryable);
			queryable = FilterByKeyword(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);
			queryable = FilterByLocalAuthority(localAuthorities, queryable);
			queryable = FilterByAdvisoryBoardDates(advisoryBoardDates, queryable);

			var totalProjects = queryable.Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync();

			return (projects, totalProjects);
		}

		public async Task<(IEnumerable<IProject> projects, int totalCount)> SearchFormAMatProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates)
		{
			IQueryable<Project> queryable = dbSet;

			queryable = FilterFormAMAT(queryable);
			queryable = FilterByRegion(regions, queryable);
			queryable = FilterByStatus(states, queryable);
			queryable = FilterByKeyword(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);
			queryable = FilterByLocalAuthority(localAuthorities, queryable);
			queryable = FilterByAdvisoryBoardDates(advisoryBoardDates, queryable);

			var totalProjects = queryable.Select(p => p.FormAMatProjectId).Distinct().Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn).ToListAsync();

			return (projects, totalProjects);
		}

		public async Task<(IEnumerable<IProject> projects, int totalCount)> SearchGroupedProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates)
		{
			IQueryable<Project> queryable = dbSet;

			queryable = FilterGroupedProjects(queryable);
			queryable = FilterByRegion(regions, queryable);
			queryable = FilterByStatus(states, queryable);
			queryable = FilterByKeyword(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);
			queryable = FilterByLocalAuthority(localAuthorities, queryable);
			queryable = FilterByAdvisoryBoardDates(advisoryBoardDates, queryable);

			var totalProjects = queryable.Select(p => p.ProjectGroupId).Distinct().Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn).ToListAsync();

			return (projects, totalProjects);
		}

		private IQueryable<Project> FilterByAdvisoryBoardDates(IEnumerable<string>? advisoryBoardDates, IQueryable<Project> queryable)
		{
			if (advisoryBoardDates != null && advisoryBoardDates.Any())
			{
				var advisoryBoardDatesToDateTime = advisoryBoardDates.Select(abd => DateTime.ParseExact(abd, "MMM yy", CultureInfo.InvariantCulture));
				queryable = queryable.Where(p =>
					p.Details.HeadTeacherBoardDate != null && advisoryBoardDatesToDateTime.Contains(EF.Functions.DateFromParts(p.Details.HeadTeacherBoardDate.Value.Year, p.Details.HeadTeacherBoardDate.Value.Month, 1)));
			}

			return queryable;
		}

		private static IQueryable<Project> FilterByLocalAuthority(IEnumerable<string>? localAuthorities, IQueryable<Project> queryable)
		{
			if (localAuthorities != null && localAuthorities.Any())
			{
				var lowerCaseRegions = localAuthorities.Select(la => la.ToLower());
				queryable = queryable.Where(p =>
					!string.IsNullOrEmpty(p.Details.LocalAuthority) && lowerCaseRegions.Contains(p.Details.LocalAuthority.ToLower()));
			}

			return queryable;
		}

		public async Task<IEnumerable<IProject>> GetConversionProjectsThatRequireFormAMatCreation(CancellationToken cancellationToken)
		{
			return await dbSet.Where(x => !x.FormAMatProjectId.HasValue && x.Details.IsFormAMat.HasValue && x.Details.IsFormAMat.Value).ToListAsync(cancellationToken).ConfigureAwait(false);
		}
		public async Task<IEnumerable<IProject>> GetConversionProjectsByFormAMatId(int? id, CancellationToken cancellationToken)
		{
			return await dbSet.Where(x => x.FormAMatProjectId == id).ToListAsync(cancellationToken).ConfigureAwait(false);
		}

		public async Task<IEnumerable<IProject>> GetConversionProjectsByFormAMatIds(IEnumerable<int?> ids, CancellationToken cancellationToken)
		{
			var formAMatProjectIds = ids.ToList();

			return await dbSet.Where(x => formAMatProjectIds.Contains(x.FormAMatProjectId)).ToListAsync(cancellationToken).ConfigureAwait(false);
		}

		public async Task<IEnumerable<IProject>> GetConversionProjectsByProjectIds(IEnumerable<int> ids, CancellationToken cancellationToken)
		{
			var projectIds = ids.ToList();

			return await dbSet.Where(x => projectIds.Contains(x.Id)).ToListAsync(cancellationToken).ConfigureAwait(false);
		}

		public async Task<IEnumerable<IProject>> GetConversionProjectsForNewGroup(string trustReferenceNumber, CancellationToken cancellationToken)
		{
			var projects = await dbSet.Where(x => x.Details.TrustReferenceNumber == trustReferenceNumber && 
			x.Details.ProjectStatus == "Converter Pre-AO (C)" && x.ProjectGroupId == null).ToListAsync(cancellationToken);

			return projects;
		}

		// Project Groups


		public async Task<IEnumerable<IProject>> GetConversionProjectsByProjectGroupIdAsync(int? projectGroupId, CancellationToken cancellationToken)
		{
			var projects = await dbSet.Where(x => x.ProjectGroupId == projectGroupId).ToListAsync(cancellationToken);

			return projects;
		}

		public async Task<IEnumerable<IProject>> GetProjectsByProjectGroupIdsAsync(IEnumerable<int> projectGroupIds, CancellationToken cancellationToken)
		{
			return await dbSet
			.Where(p => projectGroupIds.Contains(p.ProjectGroupId.GetValueOrDefault()))
			.ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<IProject>> GetProjectsByIdsOrProjectGroupIAsync(IEnumerable<int> projectIds, int? projectGroupId, CancellationToken cancellationToken)
		{
			return await dbSet
			.Where(p => p.ProjectGroupId == projectGroupId || projectIds.Contains(p.ProjectGroupId.GetValueOrDefault()))
			.ToListAsync(cancellationToken);
		}
	}
}
