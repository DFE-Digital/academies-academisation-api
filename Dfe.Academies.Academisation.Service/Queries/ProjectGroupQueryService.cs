using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService
{
	public class ProjectGroupQueryService(IProjectGroupRepository projectGroupRepository, IConversionProjectRepository conversionProjectRepository, ILogger<ProjectGroupQueryService> logger) : IProjectGroupQueryService
	{
		public async Task<PagedDataResponse<ProjectGroupResponseModel>?> GetProjectGroupsAsync(IEnumerable<string>? states, string? title, 
			IEnumerable<string>? deliveryOfficers,  IEnumerable<string>? regions, 
			IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count,CancellationToken cancellationToken)
		{
			//var (conversionProjects, _) = await conversionProjectRepository.SearchProjectsV2(null, searchModel.Title, null, null, null, null, searchModel.Page, searchModel.Count);
			//var (projectGroups, totalCount) = await projectGroupRepository.SearchProjectGroups(searchModel.Page, searchModel.Count,
			//	searchModel.ReferenceNumber, CombineTrustReferences(conversionProjects.Select(x => x.Details.TrustReferenceNumber), searchModel.TrustReference), cancellationToken);

			//var conversionsProjects = await conversionProjectRepository.GetProjectsByProjectGroupAsync(projectGroups.Select(x => x.Id).ToList(), cancellationToken);
			//var response = MapToResponse(projectGroups, conversionsProjects);

			//var pageResponse = PagingResponseFactory.Create("project-groups/groups", searchModel.Page, searchModel.Count, totalCount, []);
			//return new PagedDataResponse<ProjectGroupResponseModel>(response, pageResponse);
			var (projects, totalCount) = await conversionProjectRepository.SearchGroupedProjects(states, title, deliveryOfficers, regions, localAuthorities, advisoryBoardDates);

			var projectGroupAggregates = await projectGroupRepository.GetByIds(projects
				.Select(x => x.ProjectGroupId).Distinct(), cancellationToken).ConfigureAwait(false);

			if (!states.Any() && string.IsNullOrWhiteSpace(title) && !deliveryOfficers.Any() && !regions.Any() && !localAuthorities.Any() && !advisoryBoardDates.Any()) {
				projectGroupAggregates = await projectGroupRepository.GetAll();
			}

			// doing paging here as it has to be at a form a mat level
			projectGroupAggregates = projectGroupAggregates.OrderByDescending(x => x.CreatedOn).Skip((page - 1) * count)
					.Take(count).ToList();

			// need to add back in any projects that were filtered out, otherwise the project list indicates there are less projects in the group than there really is
			projects = projects.Union(await conversionProjectRepository.GetProjectsByProjectGroupIdsAsync(projectGroupAggregates.Select(x => x.Id).Cast<int>(), cancellationToken));

			var pageResponse = PagingResponseFactory.Create("project-groups/groups", page, count, totalCount,
				new Dictionary<string, object?> {
				{"states", states},
				});

			var data = projectGroupAggregates.Select(p => p.MapToProjectGroupServiceModel(projects));

			return new PagedDataResponse<ProjectGroupResponseModel>(data,
				pageResponse);
		}

		private IEnumerable<string?> CombineTrustReferences(IEnumerable<string?> trustReferences, string? trustReference)
		{
			var trustReferencesList = trustReferences.IsNullOrEmpty() ? [] : trustReferences;
			return trustReference.IsNullOrEmpty() ? trustReferencesList : trustReferencesList.Concat([trustReference]);
		}
	}
}
