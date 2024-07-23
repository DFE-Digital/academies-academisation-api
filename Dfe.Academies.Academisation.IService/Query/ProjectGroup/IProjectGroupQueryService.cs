using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;

namespace Dfe.Academies.Academisation.IService.Query.ProjectGroup
{
	public interface IProjectGroupQueryService
	{
		Task<ProjectGroupServiceModel?> GetProjectGroupByUrn(string urn, CancellationToken cancellationToken);
	}
}
