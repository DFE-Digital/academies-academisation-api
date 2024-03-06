using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
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

		public async Task<IFormAMatProject?> GetByApplicationReference(string? applicationReferenceNumber, CancellationToken cancellationToken)
		{
			return await this.dbSet.SingleOrDefaultAsync(x => x.ApplicationReference == applicationReferenceNumber, cancellationToken).ConfigureAwait(false);
		}

		public async Task<List<IFormAMatProject>> GetByIds(IEnumerable<int?> formAMatProjectIds, CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => formAMatProjectIds.Contains(x.Id)).Cast<IFormAMatProject>().ToListAsync(cancellationToken).ConfigureAwait(false);
		}
		public async Task<List<IFormAMatProject>> GetProjectsWithoutReference(CancellationToken cancellationToken)
		{
			return await this.dbSet.Where(x => x.ReferenceNumber == null || x.ReferenceNumber == "").Cast<IFormAMatProject>().ToListAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
