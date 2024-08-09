using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, ILogger<SetProjectGroupCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupCommand, CommandResult>
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

			var projects = await conversionProjectRepository.GetProjectsByIdsAsync(message.ConversionProjectIds, cancellationToken).ConfigureAwait(false);
			var groupConversionProjects = await conversionProjectRepository.GetConversionProjectsByProjectGroupIdAsync(projectGroup.Id, cancellationToken).ConfigureAwait(false);
			var conversionProjects = GetConversionProjects(projects, groupConversionProjects);

			if (conversionProjects != null && conversionProjects.Any())
			{
				logger.LogInformation("Setting conversions with project group id:{value}", projectGroup.Id);

				var removedConversionProjects = conversionProjects.Where(x
					=> !message.ConversionProjectIds.Contains(x.Id)).ToList();
				foreach (var removedConversionProject in removedConversionProjects)
				{
					removedConversionProject.SetProjectGroupId(null);
					conversionProjectRepository.Update((Domain.ProjectAggregate.Project)removedConversionProject);
				}

				var addConversionProjects = conversionProjects.Except(removedConversionProjects).ToList();
				foreach (var addConversionProject in addConversionProjects)
				{
					addConversionProject.SetProjectGroupId(projectGroup.Id);
					conversionProjectRepository.Update((Domain.ProjectAggregate.Project)addConversionProject);
				}

				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}
			return new CommandSuccessResult();
		}

		private IEnumerable<IProject> GetConversionProjects(IEnumerable<IProject> conversionProjects, IEnumerable<IProject> groupConversionProjects)
		{
			var projects = new List<IProject>();
			if (conversionProjects == null && !conversionProjects!.Any() && groupConversionProjects == null && !groupConversionProjects!.Any())
			{
				return Enumerable.Empty<IProject>();
			}

			if (conversionProjects != null && conversionProjects.Any())
			{
				projects.AddRange(conversionProjects);
			}

			if (groupConversionProjects != null && groupConversionProjects.Any())
			{
				projects.AddRange(groupConversionProjects);
			}

			return projects;
		}
	}
}
