using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupAssignUserCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, ILogger<SetProjectGroupAssignUserCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupAssignUserCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetProjectGroupAssignUserCommand message, CancellationToken cancellationToken)
		{
			logger.LogError($"Setting project group with reference number:{message.GroupReferenceNumber}");

			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(message.GroupReferenceNumber, cancellationToken);
			if (projectGroup == null)
			{
				logger.LogError($"Project group is not found with reference number:{message.GroupReferenceNumber}");
				return new NotFoundCommandResult();
			}
			projectGroup.SetAssignedUser(message.UserId, message.FullName, message.EmailAddress);
			projectGroupRepository.Update(projectGroup);

			var conversionProjects = await conversionProjectRepository.GetConversionProjectsByProjectGroupIdAsync(projectGroup.Id, cancellationToken);

			foreach (var conversionProject in conversionProjects)
			{
				if (conversionProject is null)
				{
					logger.LogError($"Conversion project not found with project group id: {projectGroup.Id}");
					return new NotFoundCommandResult();
				}
				if (conversionProject.Details.AssignedUser == null)
				{
					conversionProject.SetAssignedUser(message.UserId, message.FullName, message.EmailAddress);

					conversionProjectRepository.Update(conversionProject as Project);
				}
			}

			await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			 
			return new CommandSuccessResult();
		}
	}
}
