using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public interface IApplicationRepository : IRepository<Application>
{
	public Task<IEnumerable<IApplication>> GetAllAsync();
	public Task<IApplication?> GetByIdAsync(object id);
	public Task Insert(IApplication obj);
	public void Update(IApplication obj);

	public void ConcurrencySafeUpdate(IApplication obj, Guid concurrencyToken);
	public Task Delete(object id);
	public Task<List<IApplication>> GetByUserEmail(string userEmail);
	public Task<Application?> GetByApplicationReference(string applicationReference);
}
