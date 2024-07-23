using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class ProjectGroupRepository : GenericRepository<ProjectGroup>, IProjectGroupRepository
	{
		private readonly AcademisationContext _context;
		public ProjectGroupRepository(AcademisationContext context) : base(context) 
			=> _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;

		private IQueryable<ProjectGroup> DefaultIncludes()
		{
			var x = dbSet
				.Include(x => x.AssignedUser)
				.AsQueryable();

			return x;
		}

		public AcademisationContext Context() => _context;

		public async Task<ProjectGroup?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken) 
			=> await DefaultIncludes().SingleOrDefaultAsync(x => x.ReferenceNumber == referenceNumber, cancellationToken);
	}
}
