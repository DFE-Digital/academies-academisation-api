using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup; 

namespace Dfe.Academies.Academisation.IService.Query.ProjectGroup
{
	public interface IProjectGroupQueryService
	{
		Task<PagedDataResponse<ProjectGroupResponseModel>?> GetProjectGroupsAsync(ProjectGroupSearchModel searchModel, CancellationToken cancellationToken);
	}
}
