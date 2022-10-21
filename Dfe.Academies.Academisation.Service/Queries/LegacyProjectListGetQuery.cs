using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries;

public class LegacyProjectListGetQuery : ILegacyProjectListGetQuery
{
	private readonly IProjectListGetDataQuery _projectListGetDataQuery;

	public LegacyProjectListGetQuery(IProjectListGetDataQuery projectListGetDataQuery)
	{
		_projectListGetDataQuery = projectListGetDataQuery;
	}

	public async Task<LegacyApiResponse<LegacyProjectServiceModel>?> GetProjects(
		string? states, string? title, int page, int count, int? urn)
	{
		var statusList = string.IsNullOrEmpty(states)
			? null
			: states.ToLower().Split(',').ToList();

		(IEnumerable<IProject> projects, int totalCount) = await _projectListGetDataQuery.SearchProjects(statusList, title, page, count, urn);
		
		var pageResponse = PagingResponseFactory.Create("legacy/projects", page, count, totalCount, 
			new Dictionary<string, object?> {
				{"states", states},
				{"urn", urn}
			});

		return new LegacyApiResponse<LegacyProjectServiceModel>(projects.Select(p => p.MapToServiceModel()),
			pageResponse);
	}
}
