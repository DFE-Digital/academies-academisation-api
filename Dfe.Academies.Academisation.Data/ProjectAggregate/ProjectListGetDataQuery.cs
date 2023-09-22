﻿using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
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
			IQueryable<Project> queryable = _context.Projects;

			queryable = FilterByRegion(regions, queryable);
			queryable = FilterByStatus(states, queryable);
			queryable = FilterByUrn(urn, queryable);
			queryable = FilterBySchool(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);
			queryable = FilterByApplicationIds(applicationReferences, queryable);

			var totalProjects = queryable.Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.Details.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync();

			return (projects,totalProjects);
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
	}
}
