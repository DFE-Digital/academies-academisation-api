using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class TransferProjectRepository : GenericRepository<TransferProject>, ITransferProjectRepository
	{
		private readonly AcademisationContext _context;
		public TransferProjectRepository(AcademisationContext context) : base(context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public IUnitOfWork UnitOfWork => _context;
	}
}
