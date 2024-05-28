using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Dfe.Academies.Contracts.V4.Establishments;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.Legacy.Project
{
	public class EnrichTransferProjectCommand : IEnrichTransferProjectCommand
	{
		private readonly ILogger<EnrichTransferProjectCommand> _logger;
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IAcademiesQueryService _establishmentRepository;

		public EnrichTransferProjectCommand(
			ILogger<EnrichTransferProjectCommand> logger,
			ITransferProjectRepository conversionProjectRepository,
			IAcademiesQueryService establishmentRepository)
		{
			_logger = logger;
			_transferProjectRepository = conversionProjectRepository;
			_establishmentRepository = establishmentRepository;
		}

		public async Task Execute()
		{
			var incompleteProjects = await _transferProjectRepository.GetIncompleteProjects();

			if (incompleteProjects == null || !incompleteProjects.Any())
			{
				_logger.LogInformation("No transfer projects requiring enrichment found.");
				return;
			}

			foreach (var project in incompleteProjects)
			{
				foreach (var academy in project.TransferringAcademies)
				{
					EstablishmentDto? school = await _establishmentRepository.GetEstablishmentByUkprn(academy.OutgoingAcademyUkprn);

					if (school == null)
					{
						_logger.LogWarning("No schools found for project - {project}, ukprn - {ukpr}", project.Id, academy.OutgoingAcademyUkprn);
						continue;
					}

					project.SetAcademyReferenceData(academy.OutgoingAcademyUkprn, school.Gor.Name, school.LocalAuthorityName);
					
				}
				_transferProjectRepository.Update(project as Domain.TransferProjectAggregate.TransferProject);	
				_logger.LogInformation("Transfer Project {project} updated", project.Id);	
			}
			
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync();
		}
	}
}
