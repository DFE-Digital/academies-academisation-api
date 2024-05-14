using System.Threading;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class TransferProjectRepository : GenericRepository<TransferProject>, ITransferProjectRepository
	{
		private readonly AcademisationContext _context;
		public TransferProjectRepository(AcademisationContext context) : base(context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public IUnitOfWork UnitOfWork => _context;

		public async Task<ITransferProject?> GetByUrn(int urn)
		{
			return await DefaultIncludes().SingleOrDefaultAsync(x => x.Urn == urn);
		}
		public async Task<(IEnumerable<ITransferProject>, int totalcount)> SearchProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count)
		{
			IQueryable<TransferProject> queryable = DefaultIncludes();

			// Region & Local Authority isn't on Transfers right now
			//queryable = FilterByRegion(regions, queryable);
			//queryable = FilterByLocalAuthority(localAuthorities, queryable);			
			queryable = FilterByStatus(states, queryable);
			queryable = FilterByKeyword(title, queryable);
			queryable = FilterByDeliveryOfficer(deliveryOfficers, queryable);

			var totalProjects = queryable.Count();
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count).ToListAsync();

			return (projects, totalProjects);
		}
		private static IQueryable<TransferProject> FilterByDeliveryOfficer(IEnumerable<string>? deliveryOfficers, IQueryable<TransferProject> queryable)
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
		private static IQueryable<TransferProject> FilterByKeyword(string? title, IQueryable<TransferProject> queryable)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{
				queryable = queryable.Where(p =>
							p.TransferringAcademies.Any(x =>
							EF.Functions.Like(x.IncomingTrustName, $"%{title}%"))
							|| EF.Functions.Like(p.OutgoingTrustName, $"%{title}%") 
							|| EF.Functions.Like(p.Urn.ToString(), $"%{title}%")
							|| EF.Functions.Like(p.OutgoingTrustUkprn, $"%{title}%")
							|| EF.Functions.Like(p.ProjectReference, $"%{title}%")
							|| p.TransferringAcademies.Any(ta => EF.Functions.Like(ta.IncomingTrustUkprn, $"%{title}%")));  
			}

			return queryable;
		}

		private static IQueryable<TransferProject> FilterURN(string title, IQueryable<TransferProject> queryable)
		{
			return queryable.Where(p => p.Urn.ToString().Contains(title, StringComparison.CurrentCultureIgnoreCase));
		}
		private static IQueryable<TransferProject> FilterByUrn(int? urn, IQueryable<TransferProject> queryable)
		{
			if (urn.HasValue) queryable = queryable.Where(p => p.Urn == urn);

			return queryable;
		}
		private static IQueryable<TransferProject> FilterByStatus(IEnumerable<string>? states, IQueryable<TransferProject> queryable)
		{
			if (states != null && states!.Any())
			{
				queryable = queryable.Where(p => states.Contains(p.Status!.ToLower()));
			}

			return queryable;
		}
		public async Task<IEnumerable<ITransferProject?>> GetAllTransferProjects()
		{
			try
			{
				return await DefaultIncludes().ToListAsync();
			}
			catch (Exception ex)
			{
				var x = 1;
				throw;
			}


		}

		private IQueryable<TransferProject> DefaultIncludes()
		{
			var x = this.dbSet
				.Include(x => x.TransferringAcademies)
				.Include(x => x.IntendedTransferBenefits)
				.AsQueryable();

			return x;
		}

	}
}
