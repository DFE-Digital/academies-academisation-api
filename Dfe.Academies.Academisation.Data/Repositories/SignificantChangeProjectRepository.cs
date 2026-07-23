using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class SignificantChangeProjectRepository(AcademisationContext context) : GenericRepository<SignificantChangeProject>(context), ISignificantChangeProjectRepository
	{
		private readonly AcademisationContext _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;

		public async Task<(IEnumerable<SignificantChangeProject> projects, int totalCount)> SearchSignificantProjects(int page, int count, CancellationToken cancellationToken)
		{
			IQueryable<SignificantChangeProject> queryable = dbSet;

			int totalProjects = await queryable.CountAsync(cancellationToken);
			var projects = await queryable
				.OrderByDescending(acp => acp.CreatedOn)
				.Skip((page - 1) * count)
				.Take(count)
				.ToListAsync(cancellationToken);

			return (projects, totalProjects);
		}

		public async Task<SignificantChangeProject?> GetSignificantProjectById(int id, CancellationToken cancellationToken)
		{
			return await dbSet.SingleOrDefaultAsync(project => project.Id == id, cancellationToken);
		}
	}
}
