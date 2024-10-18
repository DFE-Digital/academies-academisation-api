using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class AdvisoryBoardDecisionRepository(AcademisationContext context) : GenericRepository<ConversionAdvisoryBoardDecision>(context), IAdvisoryBoardDecisionRepository
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;

		public async Task<IConversionAdvisoryBoardDecision?> GetAdvisoryBoardDecisionById(int decisionId)
		{
			return await DefaultIncludes().SingleOrDefaultAsync(x => x.Id == decisionId);
		}

		public async Task<IEnumerable<IConversionAdvisoryBoardDecision?>> GetAllAdvisoryBoardDecisions()
		{
			return await DefaultIncludes().ToListAsync();
		}

		public async Task<IEnumerable<IConversionAdvisoryBoardDecision?>> GetAllAdvisoryBoardDecisionsForTransfers()
		{
			return await DefaultIncludes().Where(x => x.AdvisoryBoardDecisionDetails.TransferProjectId != null).ToListAsync();
		}

		public async Task<ConversionAdvisoryBoardDecision?> GetConversionProjectDecsion(int projectId)
		{
			return await DefaultIncludes().SingleOrDefaultAsync(x => x.AdvisoryBoardDecisionDetails.ConversionProjectId == projectId);
		}

		public async Task<ConversionAdvisoryBoardDecision?> GetTransferProjectDecsion(int projectId)
		{
			return await DefaultIncludes().SingleOrDefaultAsync(x => x.AdvisoryBoardDecisionDetails.TransferProjectId == projectId);
		}

		private IQueryable<ConversionAdvisoryBoardDecision> DefaultIncludes()
		{
			var x = this.dbSet
				.Include(x => x.AdvisoryBoardDecisionDetails)
				.Include(x => x.WithdrawnReasons)
				.Include(x => x.DeferredReasons)
				.Include(x => x.DeclinedReasons)
				.Include(x => x.DaoRevokedReasons)
				.AsQueryable();

			return x;
		}
	}
}
