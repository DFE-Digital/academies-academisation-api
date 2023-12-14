
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
			IQueryable<Project> queryable = this.dbSet;

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
			var advisoryBoardDates = await this.dbSet
					.OrderByDescending(p => p.Details.HeadTeacherBoardDate)
					.AsNoTracking()
					.Where(p => p.Details.HeadTeacherBoardDate.HasValue)
					.Select(p => new { p.Details.HeadTeacherBoardDate.Value.Year, p.Details.HeadTeacherBoardDate.Value.Month })
					.Distinct()
					.ToListAsync()
					.ConfigureAwait(false);

			ProjectFilterParameters filterParameters = new ProjectFilterParameters
			{
				Statuses = (await this.dbSet
					.AsNoTracking()
					.Select(p => p.Details.ProjectStatus)
					.Distinct()
					.OrderBy(p => p)
					.ToListAsync())!,
				AssignedUsers = (await this.dbSet
					.OrderByDescending(p => p.Details.AssignedUser.FullName)
					.AsNoTracking()
					.Select(p => p.Details.AssignedUser.FullName)
					.Where(p => !string.IsNullOrEmpty(p))
					.Distinct()
					.ToListAsync())!,

				LocalAuthorities = (await this.dbSet
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

		public async Task<IProject?> GetConversionProject(int id)
		{
			return await this.DefaultIncludes().SingleOrDefaultAsync(x => x.Id == id);
		}

		private IQueryable<Project> DefaultIncludes()
		{
			var x = _context.Projects
				.Include(x => x.Notes)
				.AsQueryable();

			return x;
		}

		public async Task<(IEnumerable<IProject> projects, int totalCount)> SearchProjectsV2(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count)
		{
			IQueryable<Project> queryable = this.dbSet;

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

		private static IQueryable<Project> FilterByLocalAuthority(IEnumerable<string> localAuthorities, IQueryable<Project> queryable)
		{
			if (localAuthorities != null && localAuthorities.Any())
			{
				var lowerCaseRegions = localAuthorities.Select(la => la.ToLower());
				queryable = queryable.Where(p =>
					!string.IsNullOrEmpty(p.Details.LocalAuthority) && lowerCaseRegions.Contains(p.Details.LocalAuthority.ToLower()));
			}

			return queryable;
		}
	}
}
