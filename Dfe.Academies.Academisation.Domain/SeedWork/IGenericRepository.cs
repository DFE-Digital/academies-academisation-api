using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.Domain.SeedWork
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		void Insert(TEntity obj);
		Task<TEntity> GetById(int id);
		Task<IEnumerable<TEntity>> GetAll();
		void Update(TEntity obj);
		void Delete(int id);
		void Delete(TEntity entityToDelete);
	}
}
