using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, IConversionProjectRepository conversionProjectRepository, ILogger<CreateProjectGroupCommandHandler> logger, ITransferProjectRepository transferProjectRepository) : IRequestHandler<CreateProjectGroupCommand, CreateResult>
	{
		public async Task<CreateResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating project group with urn: {value}", message);

			// create project group
			var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(message.TrustReferenceNumber, message.TrustUkprn, message.TrustName, dateTimeProvider.Now);
			
			projectGroupRepository.Insert(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			projectGroup.SetProjectReference(projectGroup.Id);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			var conversionsProjectModels = new List<ConversionProjectServiceModel>();
			var transfersProjectsResponse = new List<AcademyTransferProjectResponse>();

			// if any  conversion were part of the message add them to the group
			if (message.ConversionProjectIds.Count > 0)
			{
				var conversionProjects = await conversionProjectRepository.GetConversionProjectsByProjectIds(message.ConversionProjectIds, cancellationToken).ConfigureAwait(false);

				if (conversionProjects == null || !conversionProjects.Any())
				{
					logger.LogError("No conversion projects found for the {value} Ids passed to create the group.", message.ConversionProjectIds);
					return new CreateValidationErrorResult([new ValidationError("ConversionProjectIds", "No conversion project found for the urns passed to create the group.")]);
				}

				conversionsProjectModels = await UpdateConversionProjectsWithGroupId(projectGroup.Id, conversionProjects, cancellationToken);
			}

			if (message.TransferProjectIds?.Count > 0)
			{
				var transferProjects = await transferProjectRepository.GetTransferProjectsByIdsAsync(message.TransferProjectIds!, cancellationToken).ConfigureAwait(false);
				if (transferProjects == null || !transferProjects.Any())
				{
					logger.LogError("No transfer projects found for the {value} Ids passed to create the group.", message.TransferProjectIds);
					return new CreateValidationErrorResult([new ValidationError("TransferProjectIds", "No transfer project found for the urns passed to create the group.")]);
				}

				transfersProjectsResponse = await UpdateTransferProjectsWithGroupId(projectGroup.Id, transferProjects, cancellationToken);
			}

			var responseModel = new ProjectGroupResponseModel(projectGroup.Id, projectGroup.ReferenceNumber!, projectGroup.TrustReference, projectGroup.TrustName, projectGroup.TrustUkprn, null!, conversionsProjectModels)
			{
				Transfers = transfersProjectsResponse
			};

			return new CreateSuccessResult<ProjectGroupResponseModel>(responseModel);

		}

		private async Task<List<ConversionProjectServiceModel>> UpdateConversionProjectsWithGroupId(int projectGroupId, IEnumerable<IProject> conversionProjects, CancellationToken cancellationToken)
		{
			foreach (var conversionProject in conversionProjects)
			{
				conversionProject.SetProjectGroupId(projectGroupId);

				conversionProjectRepository.Update((Domain.ProjectAggregate.Project)conversionProject);
			}

			await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return conversionProjects.Select(p => p.MapToServiceModel()).ToList();
		}

		private async Task<List<AcademyTransferProjectResponse>> UpdateTransferProjectsWithGroupId(int projectGroupId, IEnumerable<ITransferProject> transferProjects, CancellationToken cancellationToken)
		{
			foreach (var transferProject in transferProjects)
			{
				transferProject.SetProjectGroupId(projectGroupId);

				transferProjectRepository.Update((Domain.TransferProjectAggregate.TransferProject)transferProject);
			}

			await transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return transferProjects.Select(AcademyTransferProjectResponseFactory.Create).ToList();
		}
	}
}
