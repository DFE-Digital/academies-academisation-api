using AutoMapper;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class GetProjectGroupQueryCommandHandler(IProjectGroupRepository projectGroupRepository, IMapper mapper, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<GetProjectGroupQueryCommand, ProjectGroupDto?>
	{
		public async Task<ProjectGroupDto?> Handle(GetProjectGroupQueryCommand message, CancellationToken cancellationToken)
		{
			var projectGroup = await projectGroupRepository.GetByReferenceNumber(message.Urn, cancellationToken);
			if (projectGroup == null) {
				return null;
			}

			var projectGroupDto =  mapper.Map<ProjectGroupDto>(projectGroup);
			var conversionsProjects = await conversionProjectRepository.GetProjectsByProjectGroupAsync(projectGroup.Id, cancellationToken);
			if (conversionsProjects != null && conversionsProjects.Any())
			{
				projectGroupDto.ConversionsProjects = conversionsProjects.Select(x => x.Details.Urn).ToList();
			}

			return projectGroupDto;
		}
	}
}
