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

		public async Task<(IEnumerable<IProject>, int)> SearchProjects(string[]? states, string? title, string[]? deliveryOfficers, int page, int count, int? urn)
		{
			IQueryable<ProjectState> queryable = _context.Projects;

			queryable = FilterByStatus(states, queryable);
			queryable = FilterByUrn(urn, queryable);
			queryable = FilterBySchool(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);

			var totalProjects = queryable.Count();
			var projects = await queryable.OrderByDescending(acp => acp.ApplicationReceivedDate)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync();

			return (projects.Select(p => p.MapToDomain()), totalProjects);
		}

		private static IQueryable<ProjectState> FilterByStatus(string[]? states, IQueryable<ProjectState> queryable)
		{
			if (states != null && states!.Any())
			{
				queryable = queryable.Where(p => states.Contains(p.ProjectStatus!.ToLower()));
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

		private static IQueryable<ProjectState> FilterByDeliveryOfficer(string[]? deliveryOfficers, IQueryable<ProjectState> queryable)
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
