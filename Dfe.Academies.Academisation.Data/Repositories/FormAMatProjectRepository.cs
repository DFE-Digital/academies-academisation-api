using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class FormAMatProjectRepository : GenericRepository<FormAMatProject>, IFormAMatProjectRepository
	{
		private readonly AcademisationContext _context;

		public IUnitOfWork UnitOfWork => _context;
		public FormAMatProjectRepository(AcademisationContext context) : base(context)
		{
			_context = context;
		}

		public async Task<FormAMatProject> GetByApplicationReference(string? applicationReferenceNumber, CancellationToken cancellationToken)
		{
			return await this.dbSet.SingleOrDefaultAsync(x => x.ApplicationReference == applicationReferenceNumber, cancellationToken).ConfigureAwait(false);
		}
	}
}
