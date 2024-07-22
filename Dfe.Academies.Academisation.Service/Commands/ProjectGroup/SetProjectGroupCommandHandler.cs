using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using System.Linq;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, SetProjectGroupCommandValidator validator, ILogger<SetProjectGroupCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogError($"Setting project group with urn:{message.Urn}");
			var validationResult = validator.Validate(message);
			if (!validationResult.IsValid)
			{
				logger.LogError($"Validation failed while setting project group:{message}");
				return new CommandValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));
			}
			var projectGroup = await projectGroupRepository.GetByReferenceNumberAsync(message.Urn, cancellationToken);
			if (projectGroup == null)
			{
				logger.LogError($"Project group is not found with urn:{message.Urn}");
				return new NotFoundCommandResult();
			}

			projectGroup.SetProjectGroup(message.TrustReference, dateTimeProvider.Now);

			projectGroupRepository.Update(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			var conversionsProjects = await conversionProjectRepository.GetProjectsByProjectGroupAsync(projectGroup.Id, cancellationToken);
			if (conversionsProjects != null && conversionsProjects.Any())
			{
				logger.LogError($"Getting conversions with project group id:{projectGroup.Id}");

				var removedConversionProjectsUrns = conversionsProjects.Where(x 
					=> !message.ConversionProjectsUrns.Contains(x.ProjectGroupId.GetValueOrDefault())).Select(x => x.Details.Urn).ToList();
				await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(removedConversionProjectsUrns, null, dateTimeProvider.Now, cancellationToken);
				
				var addConversionProjectsUrns = message.ConversionProjectsUrns.Except(removedConversionProjectsUrns).ToList();
				await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(addConversionProjectsUrns, null, dateTimeProvider.Now, cancellationToken);

				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}


			return new CommandSuccessResult();
		}
	}
}
