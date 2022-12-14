using Dfe.Academies.Academisation.IData.Establishment;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class EnrichProjectCommand : IEnrichProjectCommand
	{
		private readonly ILogger<EnrichProjectCommand> _logger;
		private readonly IIncompleteProjectsGetDataQuery _incompleteProjectsGetDataQuery;
		private readonly IEstablishmentGetDataQuery _establishmentRepository;
		private readonly IProjectUpdateDataCommand _projectUpdateDataCommand;

		public EnrichProjectCommand(
			ILogger<EnrichProjectCommand> logger,
			IIncompleteProjectsGetDataQuery incompleteProjectsGetDataQuery,
			IEstablishmentGetDataQuery establishmentRepository,
			IProjectUpdateDataCommand projectUpdateDataCommand)
		{
			_logger = logger;
			_incompleteProjectsGetDataQuery = incompleteProjectsGetDataQuery;
			_establishmentRepository = establishmentRepository;
			_projectUpdateDataCommand = projectUpdateDataCommand;
		}

		public async Task Execute()
		{
			var incompleteProjects = await _incompleteProjectsGetDataQuery.GetIncompleteProjects();

			if (incompleteProjects == null || !incompleteProjects.Any())
			{
				_logger.LogInformation("No projects requiring enrichment found.");
				return;
			}

			foreach (var project in incompleteProjects)
			{
				var school = await _establishmentRepository.GetEstablishment(project.Details.Urn);

				if (school == null)
				{
					_logger.LogWarning("No schools found for project - {project}, urn - {urn}", project.Id, project.Details.Urn);
					continue;
				}

				var projectChanges = new LegacyProjectServiceModel
				{
					Id = project.Id, LocalAuthority = school.LocalAuthorityName, Region = school.Gor.Name
				};

				project.Update(LegacyProjectDetailsMapper.MapNonEmptyFields(projectChanges, project));

				await _projectUpdateDataCommand.Execute(project);

				_logger.LogInformation("Project {project} updated", project.Id);
			}
		}
	}
}
