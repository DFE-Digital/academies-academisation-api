using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class LegacyProjectListGetQuery : ILegacyProjectListGetQuery 
	{
		private readonly IProjectListGetDataQuery _projectListGetDataQuery;

		public LegacyProjectListGetQuery(IProjectListGetDataQuery projectListGetDataQuery)
		{
			_projectListGetDataQuery = projectListGetDataQuery;
		}
		
		public async Task<LegacyApiResponse<LegacyProjectServiceModel>> GetProjects(
			string states, int page, int count, int? urn)
		{
			var statusList = string.IsNullOrEmpty(states) 
				? null 
				: states.ToLower().Split(',').ToList();
			
			var projects = (await _projectListGetDataQuery.SearchProjects(statusList, page, count, urn)).ToList();

			var pageResponse = CreatePagingResponse(states, page, count, projects.Count, urn);
			
			return new LegacyApiResponse<LegacyProjectServiceModel>(projects.Select(p => p.MapToServiceModel()), pageResponse);
		}
		
		private static LegacyResponse CreatePagingResponse(string states, int page, int count, int recordCount, int? urn)
		{
			var pagingResponse = new LegacyResponse
			{
				RecordCount = recordCount,
				Page = page
			};

			if (recordCount != count) return pagingResponse;

			pagingResponse.NextPageUrl = $"legacy/project?states={states}page={page+1}&count={count}&urn={urn}";

			return pagingResponse;
		}
	}
}
