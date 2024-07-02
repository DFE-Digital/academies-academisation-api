using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.OpeningDateHistoryAggregate
{
	public interface IOpeningDateHistoryRepository : IRepository<OpeningDateHistory>, IGenericRepository<OpeningDateHistory>
	{
		Task<IEnumerable<OpeningDateHistory>> GetByEntityTypeAndIdAsync(string entityType, int entityId);
	}

}
