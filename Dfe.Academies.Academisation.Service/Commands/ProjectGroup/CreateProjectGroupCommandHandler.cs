using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, CreateProjectGroupCommandValidator validator, IConversionProjectRepository conversionProjectRepository) : IRequestHandler<CreateProjectGroupCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			var validationResult = validator.Validate(message);
			if (!validationResult.IsValid)
				return new CommandValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));

			var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(
			message.TrustReference,
			dateTimeProvider.Now);

			projectGroupRepository.Insert(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			projectGroup.SetProjectReference(projectGroup.Id);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			if (message.ConversionProjectsUrns.Any())
			{
				if(await conversionProjectRepository.AreProjectsAssociateToAnotherProjectGroupAsync(message.ConversionProjectsUrns, cancellationToken))
				{
					return new BadRequestCommandResult();
				}

				await conversionProjectRepository.UpdateProjectsWithProjectGroupIdAsync(message.ConversionProjectsUrns, projectGroup.Id, cancellationToken);
				await conversionProjectRepository.UnitOfWork.SaveChangesAsync();
			}

			return new CommandSuccessResult();
		}
	}
}
