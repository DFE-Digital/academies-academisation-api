using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Extensions;
using Microsoft.EntityFrameworkCore;
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

			var typeName = message.GetGenericTypeName();
			try
			{
				var strategy = projectGroupRepository.UnitOfWork.CreateExecutionStrategy();
				await strategy.ExecuteAsync(async () =>
				{
					await using var transaction = await projectGroupRepository.UnitOfWork.BeginTransactionAsync();
					using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
					{
						logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, message);
						projectGroup.SetProjectGroup(message.TrustUrn, dateTimeProvider.Now);

						projectGroupRepository.Update(projectGroup);
						await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

						var conversionsProjects = await conversionProjectRepository.GetProjectsByProjectGroupAsync([projectGroup.Id], cancellationToken);
						if (conversionsProjects != null && conversionsProjects.Any())
						{
							logger.LogInformation($"Setting conversions with project group id:{projectGroup.Id}");

							var removedConversionProjectsUrns = conversionsProjects.Where(x
								=> !message.ConversionsUrns.Contains(x.ProjectGroupId.GetValueOrDefault())).Select(x => x.Details.Urn).ToList();
							await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(removedConversionProjectsUrns, null, dateTimeProvider.Now, cancellationToken);

							var addConversionProjectsUrns = message.ConversionsUrns.Except(removedConversionProjectsUrns).ToList();
							await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(addConversionProjectsUrns, null, dateTimeProvider.Now, cancellationToken);

							await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
						}
						await projectGroupRepository.UnitOfWork.CommitTransactionAsync();
					}
				});
				return new CommandSuccessResult();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command} - {Error})", typeName, message, ex.Message);
				throw;
			}
		}
	}
}
