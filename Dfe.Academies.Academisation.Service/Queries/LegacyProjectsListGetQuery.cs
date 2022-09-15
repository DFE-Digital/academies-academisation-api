using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class LegacyProjectsListGetQuery : ILegacyProjectsListGetQuery 
	{
		public async Task<LegacyProjectServiceModel> GetProjects(
			string states, int page, int count, int? urn)
		{
			await Task.CompletedTask;
			throw new NotImplementedException("");
		}
	}
}
