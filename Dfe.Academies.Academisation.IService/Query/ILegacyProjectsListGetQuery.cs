using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ILegacyProjectsListGetQuery
	{
		Task<LegacyProjectServiceModel> GetProjects(
			string states, int page, int count, int? urn);
	}
}
