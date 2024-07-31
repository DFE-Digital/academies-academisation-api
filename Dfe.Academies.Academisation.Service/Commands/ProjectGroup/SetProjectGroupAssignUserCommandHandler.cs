using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupAssignUserCommandHandler(IProjectGroupRepository projectGroupRepository, ILogger<SetProjectGroupAssignUserCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupAssignUserCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetProjectGroupAssignUserCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Setting project group with reference number: {0}", message.GroupReferenceNumber);

			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(message.GroupReferenceNumber, cancellationToken);
			if (projectGroup == null)
			{
				logger.LogError("Project group is not found with reference number:{0}", message.GroupReferenceNumber);
				return new NotFoundCommandResult();
			}
			projectGroup.SetAssignedUser(message.UserId, message.FullName, message.EmailAddress);
			projectGroupRepository.Update(projectGroup);

			var conversionProjects = await conversionProjectRepository.GetConversionProjectsByProjectGroupIdAsync(projectGroup.Id, cancellationToken);

			foreach (var conversionProject in conversionProjects)
			{
				if (conversionProject is null)
				{
					logger.LogError("Conversion project not found with project group id: {0}", projectGroup.Id);
					return new NotFoundCommandResult();
				}
				if (conversionProject.Details.AssignedUser == null)
				{
					conversionProject.SetAssignedUser(message.UserId, message.FullName, message.EmailAddress);

					conversionProjectRepository.Update((Project)conversionProject);
				}
			}

			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			 
			return new CommandSuccessResult();
		}
	}
}
