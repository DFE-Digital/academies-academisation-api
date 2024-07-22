using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate
{
	public interface IProjectGroupRepository : IRepository<ProjectGroup>, IGenericRepository<ProjectGroup>
	{
		Task<ProjectGroup?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
	}
}
