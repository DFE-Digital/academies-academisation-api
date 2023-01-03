using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public interface IApplicationRepository : IRepository<Application>
{
	public Task<List<Application>> GetByUserEmail(string userEmail);

	public Task<IApplication?> GetApplicationByIdAsync(int id);

	public void UpdateApplication(IApplication application);

}
