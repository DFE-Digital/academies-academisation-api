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
using Microsoft.IdentityModel.Tokens;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService
{
	public class ProjectGroupQueryService(IProjectGroupRepository projectGroupRepository, IConversionProjectRepository conversionProjectRepository) : IProjectGroupQueryService
	{
		public async Task<ProjectGroupResponseModel> GetProjectGroupByIdAsync(int id, CancellationToken cancellationToken)
		{
			var projectGroup = await projectGroupRepository.GetById(id);
			var relatedProjects = await conversionProjectRepository.GetConversionProjectsByProjectGroupIdAsync(projectGroup.Id, cancellationToken).ConfigureAwait(false);

			return projectGroup.MapToProjectGroupServiceModel(relatedProjects);
		}

		public async Task<PagedDataResponse<ProjectGroupResponseModel>?> GetProjectGroupsAsync(IEnumerable<string>? states, string? title,
			IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions,
			IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count, CancellationToken cancellationToken)
		{
			IEnumerable<IProjectGroup> projectGroupAggregates = new List<IProjectGroup>();
			IEnumerable<IProject> projectAggregates = new List<IProject>();

			// No project filters return all groups
			if (!states.Any() && string.IsNullOrWhiteSpace(title) && !deliveryOfficers.Any() && !regions.Any() && !localAuthorities.Any() && !advisoryBoardDates.Any())
			{
				projectGroupAggregates = await projectGroupRepository.GetAll();
			}
			else
			{
				//get project groups based on filters
				(projectAggregates, _) = await conversionProjectRepository.SearchGroupedProjects(states, title, deliveryOfficers, regions, localAuthorities, advisoryBoardDates);

				projectGroupAggregates = await projectGroupRepository.GetByIds(projectAggregates
					.Select(x => x.ProjectGroupId).Distinct(), cancellationToken).ConfigureAwait(false);

				if (!string.IsNullOrWhiteSpace(title))
				{
					projectGroupAggregates = projectGroupAggregates.Union(await projectGroupRepository.SearchProjectGroups(title, cancellationToken));
				}
			}

			// doing paging here as it has to be at a group level
			projectGroupAggregates = projectGroupAggregates.OrderByDescending(x => x.CreatedOn).Skip((page - 1) * count)
					.Take(count).ToList();

			// need to add back in any projects that were filtered out, otherwise the project list indicates there are less projects in the group than there really is
			projectAggregates = projectAggregates.Union(await conversionProjectRepository.GetProjectsByProjectGroupIdsAsync(projectGroupAggregates.Select(x => x.Id).Cast<int>(), cancellationToken));

			var pageResponse = PagingResponseFactory.Create("project-groups/groups", page, count, projectGroupAggregates.Count(),
				new Dictionary<string, object?> {
				{"states", states},
				});

			var data = projectGroupAggregates.Select(p => p.MapToProjectGroupServiceModel(projectAggregates));

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
