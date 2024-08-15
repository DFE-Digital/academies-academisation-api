using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class DeleteProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, ILogger<DeleteProjectGroupCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Setting project group with reference number:{value}", message.GroupReferenceNumber);

			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(message.GroupReferenceNumber, cancellationToken);
			if (projectGroup == null)
			{
				logger.LogError("Project group is not found with reference number:{value}", message.GroupReferenceNumber);
				return new NotFoundCommandResult();
			}

			var conversionProjects = await conversionProjectRepository.GetProjectsByIdsAsync(message.ConversionProjectIds, cancellationToken).ConfigureAwait(false);
			if (conversionProjects != null && conversionProjects.Any())
			{
				  
			}
			return new CommandSuccessResult();
		}
	}
}
