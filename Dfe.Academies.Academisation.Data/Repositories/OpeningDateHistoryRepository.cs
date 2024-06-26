using Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class OpeningDateHistoryRepository : GenericRepository<OpeningDateHistory>, IOpeningDateHistoryRepository
	{
		private readonly AcademisationContext _context;

		public OpeningDateHistoryRepository(AcademisationContext context) : base(context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public IUnitOfWork UnitOfWork => _context;

		public async Task<IEnumerable<OpeningDateHistory>> GetByEntityTypeAndIdAsync(string entityType, int entityId)
		{
			return await _context.Set<OpeningDateHistory>()
								 .Where(odh => odh.EntityType == entityType && odh.EntityId == entityId)
								 .ToListAsync();
		}
	}
}
