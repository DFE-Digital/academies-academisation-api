using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ProjectGetStatusesQuery : IProjectGetStatusesQuery
	{
		private readonly IProjectStatusesDataQuery _projectGetStatusesQuery;

		public ProjectGetStatusesQuery(IProjectStatusesDataQuery projectGetStatusesQuery)
		{
			_projectGetStatusesQuery = projectGetStatusesQuery;
		}

		public async Task<ProjectFilterParameters> Execute()
		{
			return await _projectGetStatusesQuery.Execute();
		}
	}	
}
