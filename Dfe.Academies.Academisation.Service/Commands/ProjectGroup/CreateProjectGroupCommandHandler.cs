using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, CreateProjectGroupCommandValidator validator, IConversionProjectRepository conversionProjectRepository, ILogger<CreateProjectGroupCommandHandler> logger) : IRequestHandler<CreateProjectGroupCommand, CommandResult>
	{
		//public async Task<CommandResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		//{
		//	logger.LogError($"Creating project group with urn:{message}");
		//	var validationResult = validator.Validate(message);
		//	if (!validationResult.IsValid)
		//	{
		//		logger.LogError($"Validation failed while validating the request:{message}");
		//		return new CommandValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage))); 
		//	}

		//	if (message.ConversionsUrns.Any() && await conversionProjectRepository.AreProjectsAssociateToAnotherProjectGroupAsync(message.ConversionsUrns, cancellationToken))
		//	{
		//		logger.LogError($"Validation failed because one or more conversions are part of a different project group:{message}");
		//		return new BadRequestCommandResult();
		//	}

		//	var typeName = message.GetGenericTypeName();
		//	try
		//	{
		//		var strategy = projectGroupRepository.UnitOfWork.CreateExecutionStrategy();
		//		await strategy.ExecuteAsync(async () =>
		//		{
		//			await using var transaction = await projectGroupRepository.UnitOfWork.BeginTransactionAsync();
		//			using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
		//			{
		//				logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, message);
		//				var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(message.TrustUrn, dateTimeProvider.Now);

		//				projectGroupRepository.Insert(projectGroup);
		//				await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		//				projectGroup.SetProjectReference(projectGroup.Id);
		//				await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		//				if (message.ConversionsUrns.Any())
		//				{
		//					await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(message.ConversionsUrns, projectGroup.Id, dateTimeProvider.Now, cancellationToken);
		//				}
		//				await projectGroupRepository.UnitOfWork.CommitTransactionAsync();
		//			}
		//		});
		//		return new CommandSuccessResult();
		//	}
		//	catch (Exception ex) {
		//		logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command} - {Error})", typeName, message, ex.Message);
		//		throw;
		//	}
		//}

		public async Task<CommandResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			//	logger.LogError($"Creating project group with urn:{message}");
			//	var validationResult = validator.Validate(message);
			//	if (!validationResult.IsValid)
			//	{
			//		logger.LogError($"Validation failed while validating the request:{message}");
			//		return new CommandValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage))); 
			//	}

			//	if (message.ConversionsUrns.Any() && await conversionProjectRepository.AreProjectsAssociateToAnotherProjectGroupAsync(message.ConversionsUrns, cancellationToken))
			//	{
			//		logger.LogError($"Validation failed because one or more conversions are part of a different project group:{message}");
			//		return new BadRequestCommandResult();
			//	}

			var conversionProjects = await conversionProjectRepository.GetConversionProjectsByUrns(message.ConversionsUrns, cancellationToken).ConfigureAwait(false);

			if (conversionProjects == null || !conversionProjects.Any())
			{
				logger.LogInformation($"No conversion projects found which require form a mat projects creating.");
				return new NotFoundCommandResult();
			}

			foreach (var conversionProject in conversionProjects)
			{
				// create project group
				var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(message.TrustUrn, dateTimeProvider.Now);

				projectGroupRepository.Insert(projectGroup);
				await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				conversionProject.SetProjectGroupId(projectGroup.Id);

				conversionProjectRepository.Update(conversionProject as Domain.ProjectAggregate.Project);
			}


			await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);


			// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
