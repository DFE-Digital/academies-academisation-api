using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class EnrichTransferProjectCommand(
		ILogger<EnrichTransferProjectCommand> logger,
		ITransferProjectRepository conversionProjectRepository,
		IAcademiesQueryService establishmentRepository) : IEnrichTransferProjectCommand
	{
		public async Task Execute()
		{
			var incompleteProjects = await conversionProjectRepository.GetIncompleteProjects();

			if (incompleteProjects == null || !incompleteProjects.Any())
			{
				logger.LogInformation("No transfer projects requiring enrichment found.");
				return;
			}

			foreach (var project in incompleteProjects)
			{
				foreach (var academy in project.TransferringAcademies)
				{
					EstablishmentDto? school = await establishmentRepository.GetEstablishmentByUkprn(academy.OutgoingAcademyUkprn);

					if (school == null)
					{
						logger.LogWarning("No schools found for project - {Project}, ukprn - {Ukpr}", project.Id, academy.OutgoingAcademyUkprn);
						continue;
					}

					project.SetAcademyReferenceData(academy.OutgoingAcademyUkprn, school.Gor.Name, school.LocalAuthorityName);
					
				}
				conversionProjectRepository.Update((project as Domain.TransferProjectAggregate.TransferProject)!);	
				logger.LogInformation("Transfer Project {Project} updated", project.Id);	
			}
			
			await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
		}
	}
}
