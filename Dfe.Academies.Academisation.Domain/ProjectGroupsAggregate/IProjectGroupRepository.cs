using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate
{
	public interface IProjectGroupRepository : IRepository<ProjectGroup>, IGenericRepository<ProjectGroup>
	{
		Task<ProjectGroup?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
		Task<(IEnumerable<IProjectGroup>, int totalcount)> SearchProjectGroups(int page, int count, string? urn, string? trustUrn, string? trustName, string? academyName, string? academyUkprn, string? companiesHouseNo, CancellationToken cancellationToken);

	}
}
