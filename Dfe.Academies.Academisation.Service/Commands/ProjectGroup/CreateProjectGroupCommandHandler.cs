using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, IConversionProjectRepository conversionProjectRepository, ILogger<CreateProjectGroupCommandHandler> logger) : IRequestHandler<CreateProjectGroupCommand, CreateResult>
	{
		public async Task<CreateResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating project group with urn: {0}", message);

			// create project group
			var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(message.TrustReferenceNumber, dateTimeProvider.Now);
			
			projectGroupRepository.Insert(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			projectGroup.SetProjectReference(projectGroup.Id);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			var conversionsProjectModels = new List<ConversionsResponseModel>();

			// if any  conversion were part of the message add them to the group
			if (message.ConversionProjectIds.Any())
			{
				var conversionProjects = await conversionProjectRepository.GetConversionProjectsByIdsAsync(message.ConversionProjectIds, projectGroup.Id, cancellationToken).ConfigureAwait(false);

				if (conversionProjects == null || !conversionProjects.Any())
				{
					logger.LogError("No conversion projects found for the {0} Ids passed to create the group.", message.ConversionProjectIds);
					return new CreateValidationErrorResult([new ValidationError("ConversionProjectIds", "No conversion projects found for the urns passed to create the group.")]);
				}

				foreach (var conversionProject in conversionProjects)
				{
					conversionProject.SetProjectGroupId(projectGroup.Id);

					conversionProjectRepository.Update((Domain.ProjectAggregate.Project)conversionProject);
				}
				
				await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				conversionsProjectModels = conversionProjects.Select(p => new ConversionsResponseModel(p.Details.Urn, p.Details.SchoolName!)).ToList();
			}
		
			var responseModel = new ProjectGroupResponseModel(projectGroup.ReferenceNumber!, projectGroup.TrustReference, conversionsProjectModels);

			return new CreateSuccessResult<ProjectGroupResponseModel>(responseModel);

		}
	}
}
