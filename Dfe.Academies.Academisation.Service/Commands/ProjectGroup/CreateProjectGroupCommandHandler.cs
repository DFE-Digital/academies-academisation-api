using MediatR;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, CreateProjectGroupCommandValidator validator) : IRequestHandler<CreateProjectGroupCommand, CommandResult>
	{

		public async Task<CommandResult> Handle(CreateProjectGroupCommand message, CancellationToken cancellationToken)
		{
			var validationResult = validator.Validate(message);
			if (!validationResult.IsValid)
				return new CommandValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));

			var projectGroup = Domain.ProjectGroupsAggregate.ProjectGroup.Create(
			message.TrustReference,
			message.ReferenceNumber,
			dateTimeProvider.Now);

			projectGroupRepository.Insert(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
