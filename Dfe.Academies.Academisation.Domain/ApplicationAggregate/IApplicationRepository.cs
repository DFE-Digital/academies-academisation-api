using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public interface IApplicationRepository : IRepository<Application>
{
	public Task<List<Application>> GetByUserEmail(string userEmail);
}
