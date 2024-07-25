using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectGroupAggregate;
using Dfe.Academies.Academisation.IService.Query.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Factories;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService
{
	public class ProjectGroupQueryService(IProjectGroupRepository projectGroupRepository, IConversionProjectRepository conversionProjectRepository, ILogger<ProjectGroupQueryService> logger) : IProjectGroupQueryService
	{
		public async Task<PagedDataResponse<ProjectGroupResponseModel>?> GetProjectGroupsAsync(ProjectGroupSearchModel searchModel, CancellationToken cancellationToken)
		{
			logger.LogError($"Searching project group with :{searchModel}");

			var(projectGroups, totalCount) = await projectGroupRepository.SearchProjectGroups(searchModel.Page, searchModel.Count, 
				searchModel.Urn, searchModel.TrustUrn, searchModel.TrustName, searchModel.AcademyName, searchModel.AcademyUkprn,
				searchModel.CompaniesHouseNo, cancellationToken);

			var conversionsProjects = await conversionProjectRepository.GetProjectsByProjectGroupAsync(projectGroups.Select(x => x.Id).ToList(), cancellationToken);
			var response = MapToResponse(projectGroups, conversionsProjects);

			var pageResponse = PagingResponseFactory.Create("project-groups/groups", searchModel.Page, searchModel.Count, totalCount, []);
			return new PagedDataResponse<ProjectGroupResponseModel>(response, pageResponse);
		}

		private IEnumerable<ProjectGroupResponseModel> MapToResponse(IEnumerable<IProjectGroup> projectGroups, IEnumerable<IProject>? conversionsProjects)
		{
			return projectGroups.Select(x => new ProjectGroupResponseModel(x.ReferenceNumber!, x.TrustReference,
					conversionsProjects == null ? [] : conversionsProjects.Where(c => c.ProjectGroupId.GetValueOrDefault() == x.Id)
					.Select(p => new ConversionsResponseModel(p.Details.Urn, p.Details.SchoolName!)))).ToList();
		}
	}
}
