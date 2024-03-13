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
