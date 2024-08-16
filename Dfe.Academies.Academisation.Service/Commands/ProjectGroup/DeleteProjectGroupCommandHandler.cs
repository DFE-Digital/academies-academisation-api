using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class DeleteProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, ILogger<DeleteProjectGroupCommandHandler> logger) : IRequestHandler<DeleteProjectGroupCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(DeleteProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Setting project group with reference number:{value}", message.GroupReferenceNumber);

			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(message.GroupReferenceNumber, cancellationToken);
			if (projectGroup == null)
			{
				logger.LogError("Project group is not found with reference number:{value}", message.GroupReferenceNumber);
				return new NotFoundCommandResult();
			}

			projectGroupRepository.Delete(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult(); 
		}
	}
}
