using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ProjectGetStatusesQuery : IProjectGetStatusesQuery
	{
		private readonly IProjectStatusesDataQuery _projectGetStatusesQuery;

		public ProjectGetStatusesQuery(IProjectStatusesDataQuery projectGetStatusesQuery)
		{
			_projectGetStatusesQuery = projectGetStatusesQuery;
		}

		public async Task<List<string?>> Execute()
		{
			return await _projectGetStatusesQuery.Execute();
		}
	}	
}
