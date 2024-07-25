using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Microsoft.Extensions.Logging;
using Dfe.Academies.Academisation.Service.Extensions;
using FluentValidation;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, IValidator<CreateProjectGroupCommand> validator, IConversionProjectRepository conversionProjectRepository, ILogger<CreateProjectGroupCommandHandler> logger) : IRequestHandler<CreateProjectGroupCommand, CreateResult>
	{
		public async Task<CreateResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			logger.LogError($"Creating project group with urn:{message}");
			var validationResult = validator.Validate(message);
			if (!validationResult.IsValid)
			{
				logger.LogError($"Validation failed while validating the request:{message}");
				return new CreateValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage))); 
			}

			if (message.ConversionsUrns.Any())
			{
				var converstions = await conversionProjectRepository.GetConversionProjectsForGroup(message.TrustUrn, cancellationToken);
				if (converstions == null) {
					logger.LogError($"Validation failed because one or more conversions are part of a different project group:{message}");
					return new CreateValidationErrorResult([new ValidationError("ConversionsUrns", "One or more conversions are part of a different project group")]);
				} }

			var typeName = message.GetGenericTypeName();
			try
			{
				var responseModel = new ProjectGroupResponseModel("", message.TrustUrn, []);
				var strategy = projectGroupRepository.UnitOfWork.CreateExecutionStrategy();
				//await strategy.ExecuteAsync(async () =>
				//{
					await using var transaction = await projectGroupRepository.UnitOfWork.BeginTransactionAsync();
					using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
					{
						logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, message);
						var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(message.TrustUrn, dateTimeProvider.Now);

						projectGroupRepository.Insert(projectGroup);
						await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

						projectGroup.SetProjectReference(projectGroup.Id);
						await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

						if (message.ConversionsUrns.Any())
						{
							await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(message.ConversionsUrns, projectGroup.Id, dateTimeProvider.Now, cancellationToken);
						}
						await projectGroupRepository.UnitOfWork.CommitTransactionAsync();

						var conversionsProjects = await conversionProjectRepository.GetConversionProjectsForGroup(message.TrustUrn, cancellationToken);

					responseModel = new ProjectGroupResponseModel(projectGroup.ReferenceNumber!, projectGroup.TrustReference, conversionsProjects!.Select(p => new ConversionsResponseModel(p.Details.Urn, p.Details.SchoolName!)));
					}
				//});
				return new CreateSuccessResult<ProjectGroupResponseModel>(responseModel);
			}
			catch (Exception ex) {
				logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command} - {Error})", typeName, message, ex.Message);
				throw;
			}
		}
	}
}
