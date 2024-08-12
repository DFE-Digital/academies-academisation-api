using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using FluentValidation;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, ILogger<SetProjectGroupCommandHandler> logger, IConversionProjectRepository conversionProjectRepository, ITransferProjectRepository transferProjectRepository) : IRequestHandler<SetProjectGroupCommand, CommandResult>
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
			 
			await UpdateConversionProjects(projectGroup.Id, message.ConversionProjectIds, cancellationToken);
			await UpdateTransferProjects(projectGroup.Id, message.TransferProjectIds, cancellationToken);
			return new CommandSuccessResult();
		}

		private async Task UpdateTransferProjects(int projectGroupId, List<int>? transferProjectIds, CancellationToken cancellationToken)
		{
			if (transferProjectIds != null && transferProjectIds.Count != 0)
			{
				var projects = await transferProjectRepository.GetTransferProjectsByIdsAsync(transferProjectIds, cancellationToken);
				var groupTransferProjects = await transferProjectRepository.GetProjectsByProjectGroupIdAsync(projectGroupId, cancellationToken).ConfigureAwait(false);
				var transferProjects = GetTransferProjects(projects, groupTransferProjects);
				if (transferProjects != null && transferProjects.Any())
				{
					logger.LogInformation("Setting conversions with project group id:{value}", projectGroupId);

					var removedTransferProjects = transferProjects.Where(x
						=> !transferProjectIds.Contains(x.Id)).ToList();
					foreach (var removedTransferProject in removedTransferProjects)
					{
						removedTransferProject.SetProjectGroupId(null);
						transferProjectRepository.Update((Domain.TransferProjectAggregate.TransferProject)removedTransferProject);
					}

					var addTransferProjects = transferProjects.Except(removedTransferProjects).ToList();
					foreach (var addTransferProject in addTransferProjects)
					{
						addTransferProject.SetProjectGroupId(projectGroupId);
						transferProjectRepository.Update((Domain.TransferProjectAggregate.TransferProject)addTransferProject);
					}

					await transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
				}
			}
		}


		private async Task UpdateConversionProjects(int projectGroupId, List<int> conversionProjectIds, CancellationToken cancellationToken)
		{
			var projects = await conversionProjectRepository.GetProjectsByIdsAsync(conversionProjectIds, cancellationToken).ConfigureAwait(false);
			var groupConversionProjects = await conversionProjectRepository.GetConversionProjectsByProjectGroupIdAsync(projectGroupId, cancellationToken).ConfigureAwait(false);
			var conversionProjects = GetConversionProjects(projects, groupConversionProjects);
			if (conversionProjects != null && conversionProjects.Any())
			{
				logger.LogInformation("Setting conversions with project group id:{value}", projectGroupId);

				var removedConversionProjects = conversionProjects.Where(x
					=> !conversionProjectIds.Contains(x.Id)).ToList();
				foreach (var removedConversionProject in removedConversionProjects)
				{
					removedConversionProject.SetProjectGroupId(null);
					conversionProjectRepository.Update((Domain.ProjectAggregate.Project)removedConversionProject);
				}

				var addConversionProjects = conversionProjects.Except(removedConversionProjects).ToList();
				foreach (var addConversionProject in addConversionProjects)
				{
					addConversionProject.SetProjectGroupId(projectGroupId);
					conversionProjectRepository.Update((Domain.ProjectAggregate.Project)addConversionProject);
				}

				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}
		}

		private static IEnumerable<IProject> GetConversionProjects(IEnumerable<IProject> conversionProjects, IEnumerable<IProject> groupConversionProjects)
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

		private static IEnumerable<ITransferProject> GetTransferProjects(IEnumerable<ITransferProject> transferProjects, IEnumerable<ITransferProject> groupTransferProjects)
		{
			var projects = new List<ITransferProject>();
			if (transferProjects == null && !transferProjects!.Any() && groupTransferProjects == null && !groupTransferProjects!.Any())
			{
				return Enumerable.Empty<ITransferProject>();
			}

			if (transferProjects != null && transferProjects.Any())
			{
				projects.AddRange(transferProjects);
			}

			if (groupTransferProjects != null && groupTransferProjects.Any())
			{
				projects.AddRange(groupTransferProjects);
			}

			return projects;
		}
	}
}
