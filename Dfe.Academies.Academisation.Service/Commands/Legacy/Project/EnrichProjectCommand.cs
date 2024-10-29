using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.Academies.Contracts.V4.Trusts;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class EnrichProjectCommand(
		ILogger<EnrichProjectCommand> logger,
		IConversionProjectRepository conversionProjectRepository,
		IAcademiesQueryService establishmentRepository,
		IProjectUpdateDataCommand projectUpdateDataCommand) : IEnrichProjectCommand
	{
		public async Task Execute()
		{
			var incompleteProjects = await conversionProjectRepository.GetIncompleteProjects();

			if (incompleteProjects == null || !incompleteProjects.Any())
			{
				logger.LogInformation("No projects requiring enrichment found.");
				return;
			}
			
			foreach (var project in incompleteProjects)
			{
				EstablishmentDto? school = await establishmentRepository.GetEstablishment(project.Details.Urn);
				TrustDto trust = await establishmentRepository.GetTrustByReferenceNumber(project.Details.TrustReferenceNumber);
				
				
				if (school == null)
				{
					logger.LogWarning("No schools found for project - {project}, urn - {urn}", project.Id, project.Details.Urn);
					continue;
				}
				
				if (trust == null)
				{
					logger.LogWarning("No trusts found for project - {project}, urn - {urn}", project.Id, project.Details.Urn);
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

				await projectUpdateDataCommand.Execute(project);

				logger.LogInformation("Project {project} updated", project.Id);
			}
		}
	}
}
