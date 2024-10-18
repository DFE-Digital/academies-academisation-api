using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.CompleteTransmissionLog;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class CompleteTransmissionLogRepository : GenericRepository<CompleteTransmissionLog>, ICompleteTransmissionLogRepository
	{
		private readonly AcademisationContext _context;

		public IUnitOfWork UnitOfWork => _context;
		public CompleteTransmissionLogRepository(AcademisationContext context) : base(context)
		{
			_context = context;
		}
	}
}
