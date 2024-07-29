using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, ILogger<SetProjectGroupCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogError($"Setting project group with reference number:{message.GroupReferenceNumber}");

			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(message.GroupReferenceNumber, cancellationToken);
			if (projectGroup == null)
			{
				logger.LogError($"Project group is not found with reference number:{message.GroupReferenceNumber}");
				return new NotFoundCommandResult();
			}

			var conversionProjects = await conversionProjectRepository.GetConversionProjectsByIds(message.ConversionProjectIds, cancellationToken).ConfigureAwait(false);

			if (conversionProjects != null && conversionProjects.Any())
			{
				logger.LogInformation($"Setting conversions with project group id:{projectGroup.Id}");

				var removedConversionProjects = conversionProjects.Where(x
					=> !message.ConversionProjectIds.Contains(x.Id)).ToList();
				foreach (var removedConversionProject in removedConversionProjects)
				{
					removedConversionProject.SetProjectGroupId(null);
					conversionProjectRepository.Update(removedConversionProject as Domain.ProjectAggregate.Project);
				}

				var addConversionProjects = conversionProjects.Except(removedConversionProjects).ToList();
				foreach (var addConversionProject in addConversionProjects)
				{
					addConversionProject.SetProjectGroupId(projectGroup.Id);
					conversionProjectRepository.Update(addConversionProject as Domain.ProjectAggregate.Project);
				}

				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}
			return new CommandSuccessResult();
		}
	}
}
