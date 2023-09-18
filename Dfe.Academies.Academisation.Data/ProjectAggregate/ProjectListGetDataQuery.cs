using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class ProjectListGetDataQuery : IProjectListGetDataQuery
	{
		private readonly AcademisationContext _context;

		public ProjectListGetDataQuery(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<(IEnumerable<IProject>, int)> SearchProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions = default, IEnumerable<string>? applicationReferences = default)
		{
			IQueryable<ProjectState> queryable = _context.Projects;

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

			return (projects.Select(p => p.MapToDomain()), totalProjects);
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
				queryable = queryable.Where(p => states.Contains(p.ProjectStatus!.ToLower()));
			}

			return queryable;
		}
		private static IQueryable<ProjectState> FilterByApplicationIds(IEnumerable<string>? applicationIds, IQueryable<ProjectState> queryable)
		{
			if (applicationIds != null && applicationIds!.Any())
			{
				queryable = queryable.Where(p => applicationIds.Contains(p.ApplicationReferenceNumber!));
			}

			return queryable;
		}

		private static IQueryable<ProjectState> FilterByUrn(int? urn, IQueryable<ProjectState> queryable)
		{
			if (urn.HasValue) queryable = queryable.Where(p => p.Urn == urn);

			return queryable;
		}

		private static IQueryable<ProjectState> FilterBySchool(string? title, IQueryable<ProjectState> queryable)
		{
			if (!string.IsNullOrWhiteSpace(title)) queryable = queryable.Where(p => p.SchoolName!.ToLower().Contains(title!.ToLower()));

			return queryable;
		}

		private static IQueryable<ProjectState> FilterByDeliveryOfficer(IEnumerable<string>? deliveryOfficers, IQueryable<ProjectState> queryable)
		{
			if (deliveryOfficers != null && deliveryOfficers.Any())
			{
				var lowerCaseDeliveryOfficers = deliveryOfficers.Select(officer => officer.ToLower());

				if (lowerCaseDeliveryOfficers.Contains("not assigned"))
				{
					// Query by unassigned or assigned delivery officer
					queryable = queryable.Where(p =>
								(!string.IsNullOrEmpty(p.AssignedUserFullName) && lowerCaseDeliveryOfficers.Contains(p.AssignedUserFullName.ToLower()))
								|| string.IsNullOrEmpty(p.AssignedUserFullName));
				}
				else
				{
					// Query by assigned delivery officer only
					queryable = queryable.Where(p =>
						!string.IsNullOrEmpty(p.AssignedUserFullName) && lowerCaseDeliveryOfficers.Contains(p.AssignedUserFullName.ToLower()));
				}
			}

			return queryable;
		}
	}
}
