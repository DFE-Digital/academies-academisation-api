namespace Dfe.Academies.Academisation.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
