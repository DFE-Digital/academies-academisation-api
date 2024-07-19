using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
namespace Dfe.Academies.Academisation.Data.Repositories
{
	public class ProjectGroupRepository : GenericRepository<ProjectGroup>, IProjectGroupRepository
	{
		private readonly AcademisationContext _context;
		public ProjectGroupRepository(AcademisationContext context) : base(context) 
			=> _context = context ?? throw new ArgumentNullException(nameof(context));

		public IUnitOfWork UnitOfWork => _context;
	}
}
