using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

		public async Task<IEnumerable<ITransferProject?>> GetAllTransferProjects()
		{
			return await DefaultIncludes().ToListAsync();
		}

		private IQueryable<TransferProject> DefaultIncludes()
		{
			var x = this.dbSet
				.Include(x => x.TransferringAcademies)
				.Include(x => x.IntendedTransferBenefits)
				.AsQueryable();

			return x;
		}

		public async Task<IEnumerable<ITransferProject?>> GetAllTransferProjectsWhereTrustNameIsNull()
		{
			return await DefaultIncludes().Where(x => x.OutgoingTrustName == null || x.TransferringAcademies.Any(ta => ta.IncomingTrustName == null)).ToListAsync();
		}
	}
}
