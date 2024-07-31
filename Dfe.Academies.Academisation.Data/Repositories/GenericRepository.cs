using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		internal AcademisationContext context;
		internal DbSet<TEntity> dbSet;

		public GenericRepository(AcademisationContext context)
		{
			this.context = context;
			dbSet = context.Set<TEntity>();
		}

		public void Insert(TEntity obj)
		{
			dbSet.Add(obj);
		}

		public async Task<IEnumerable<TEntity>> GetAll()
		{
			return await dbSet.ToListAsync().ConfigureAwait(false);
		}

		public async Task<TEntity> GetById(int id)
		{
			return await dbSet.FindAsync(id).ConfigureAwait(false);
		}

		public void Delete(int id)
		{
			TEntity entityToDelete = dbSet.Find(id);
			Delete(entityToDelete);
		}

		public void Delete(TEntity entityToDelete)
		{
			if (context.Entry(entityToDelete).State == EntityState.Detached)
			{
				dbSet.Attach(entityToDelete);
			}
			dbSet.Remove(entityToDelete);
		}

		public void Update(TEntity obj)
		{
			dbSet.Attach(obj);
			context.Entry(obj).State = EntityState.Modified;
		}
	}
}
