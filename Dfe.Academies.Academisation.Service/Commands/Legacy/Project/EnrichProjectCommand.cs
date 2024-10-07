using System.Net.Http.Json;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Academies;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.Academisation.CorrelationIdMiddleware;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class EnrichProjectCommand : IEnrichProjectCommand
	{
		private readonly ILogger<EnrichProjectCommand> _logger;
		private readonly IConversionProjectRepository _conversionProjectRepository;
		private readonly IAcademiesQueryService _establishmentRepository;
		private readonly IProjectUpdateDataCommand _projectUpdateDataCommand;

		public EnrichProjectCommand(
			ILogger<EnrichProjectCommand> logger,
			IConversionProjectRepository conversionProjectRepository,
			IAcademiesQueryService establishmentRepository,
			IProjectUpdateDataCommand projectUpdateDataCommand)
		{
			_logger = logger;
			_conversionProjectRepository = conversionProjectRepository;
			_establishmentRepository = establishmentRepository;
			_projectUpdateDataCommand = projectUpdateDataCommand;
			
		}

		public async Task Execute()
		{
			var incompleteProjects = await _conversionProjectRepository.GetIncompleteProjects();

			if (incompleteProjects == null || !incompleteProjects.Any())
			{
				_logger.LogInformation("No projects requiring enrichment found.");
				return;
			}
			
			foreach (var project in incompleteProjects)
			{
				EstablishmentDto? school = await _establishmentRepository.GetEstablishment(project.Details.Urn);
				TrustDto trust = await _establishmentRepository.GetTrustByReferenceNumber(project.Details.TrustReferenceNumber);
				
				
				if (school == null)
				{
					_logger.LogWarning("No schools found for project - {project}, urn - {urn}", project.Id, project.Details.Urn);
					continue;
				}
				
				if (trust == null)
				{
					_logger.LogWarning("No trusts found for project - {project}, urn - {urn}", project.Id, project.Details.Urn);
					continue;
				}

				var projectChanges = new ConversionProjectServiceModel(project.Id, project.Details.Urn)
				{
				
					TrustUkprn = Int32.Parse(trust.Ukprn),
					LocalAuthority = school.LocalAuthorityName,
					Region = school.Gor.Name,
					SchoolPhase = school.PhaseOfEducation.Name,
					SchoolType = school.EstablishmentType.Name
				};

				project.Update(LegacyProjectDetailsMapper.MapNonEmptyFields(projectChanges, project));

				await _projectUpdateDataCommand.Execute(project);

				_logger.LogInformation("Project {project} updated", project.Id);
			}
		}
	}
}
