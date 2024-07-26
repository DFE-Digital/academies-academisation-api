using Microsoft.EntityFrameworkCore.Storage;

namespace Dfe.Academies.Academisation.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
	Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
	IExecutionStrategy CreateExecutionStrategy();
	Task<IDbContextTransaction> BeginTransactionAsync();
	Task CommitTransactionAsync();
}
