﻿using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate
{
	public interface IProjectGroupRepository : IRepository<ProjectGroup>, IGenericRepository<ProjectGroup>
	{
		Task<ProjectGroup?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
		Task<(IEnumerable<IProjectGroup>, int totalcount)> SearchProjectGroups(int page, int count, string? referenceNumber, IEnumerable<string?>? trustReferences, CancellationToken cancellationToken);
		Task<IEnumerable<IProjectGroup>> GetByIds(IEnumerable<int?> projectGroupIds, CancellationToken cancellationToken);

	}
}
