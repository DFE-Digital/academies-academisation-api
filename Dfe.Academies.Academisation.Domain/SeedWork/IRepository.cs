namespace Dfe.Academies.Academisation.Domain.SeedWork;

public interface IRepository<T> where T : IAggregateRoot
{
	IUnitOfWork UnitOfWork { get; }

	public Task<IEnumerable<T>> GetAllAsync();
	public Task<T?> GetByIdAsync(object id);
	public Task Insert(T obj);
	public void Update(T obj);
	public Task Delete(object id);
}
