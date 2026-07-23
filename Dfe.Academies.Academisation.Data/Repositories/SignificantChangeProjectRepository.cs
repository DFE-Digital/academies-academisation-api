using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class SignificantChangeProjectRepository(AcademisationContext context) : GenericRepository<SignificantChangeProject>(context), ISignificantChangeProjectRepository
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;
	}
}
