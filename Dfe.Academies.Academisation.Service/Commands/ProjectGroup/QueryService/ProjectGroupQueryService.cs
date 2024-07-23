using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.IService.Query.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService
{
	public class ProjectGroupQueryService(IProjectGroupRepository projectGroupRepository, IConversionProjectRepository conversionProjectRepository, ILogger<ProjectGroupQueryService> logger) : IProjectGroupQueryService
	{
		public async Task<ProjectGroupServiceModel?> GetProjectGroupByUrn(string urn, CancellationToken cancellationToken)
		{
			logger.LogError($"Getting project group with urn:{urn}");
			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(urn, cancellationToken);
			if (projectGroup == null)
			{
				return null;
			}

			var projectGroupServiceModel = new ProjectGroupServiceModel(projectGroup.ReferenceNumber!);
			var conversionsProjects = await conversionProjectRepository.GetProjectsByProjectGroupAsync(projectGroup.Id, cancellationToken);
			if (conversionsProjects != null && conversionsProjects.Any())
			{
				projectGroupServiceModel.ConversionsUrns = conversionsProjects.Select(x => x.Details.Urn).ToList();
			}

			return projectGroupServiceModel;
		}
	}
}
