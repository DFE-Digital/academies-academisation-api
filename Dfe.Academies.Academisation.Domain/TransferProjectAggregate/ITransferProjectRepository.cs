using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public interface ITransferProjectRepository : IRepository<TransferProject>, IGenericRepository<TransferProject>
	{
	}
}
