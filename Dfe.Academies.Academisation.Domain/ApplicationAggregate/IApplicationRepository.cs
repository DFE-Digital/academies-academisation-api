using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public interface IApplicationRepository : IRepository<Application>
{
	public Task DeleteChildObjectById<T>(object id) where T : class;
}
