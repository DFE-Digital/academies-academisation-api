using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Extensions;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, IValidator<SetProjectGroupCommand> validator, ILogger<SetProjectGroupCommandHandler> logger, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<SetProjectGroupCommand, CommandResult>
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

			projectGroup.SetProjectGroup(message.TrustUrn, dateTimeProvider.Now);

			projectGroupRepository.Update(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			var conversionProjects = await conversionProjectRepository.GetConversionProjectsByUrns(message.ConversionsUrns, cancellationToken).ConfigureAwait(false);

			if (conversionProjects != null && conversionProjects.Any())
			{
				logger.LogInformation($"Setting conversions with project group id:{projectGroup.Id}");

				var removedConversionProjects = conversionProjects.Where(x
					=> !message.ConversionsUrns.Contains(x.Details.Urn)).ToList();
				foreach (var removedConversionProject in removedConversionProjects)
				{
					removedConversionProject.SetProjectGroupId(null);
					conversionProjectRepository.Update(removedConversionProject as Domain.ProjectAggregate.Project);
				}

				var addConversionProjects = conversionProjects.Except(removedConversionProjects).ToList();
				foreach (var addConversionProject in addConversionProjects)
				{
					addConversionProject.SetProjectGroupId(projectGroup.Id);
					conversionProjectRepository.Update(addConversionProject as Domain.ProjectAggregate.Project);
				}

				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}
			return new CommandSuccessResult();
		}
	}
}
