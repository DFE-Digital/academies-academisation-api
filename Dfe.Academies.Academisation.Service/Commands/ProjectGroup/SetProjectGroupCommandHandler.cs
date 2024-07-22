using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandler(IProjectGroupRepository projectGroupRepository, IDateTimeProvider dateTimeProvider, SetProjectGroupCommandValidator validator, ILogger<SetProjectGroupCommandHandler> _logger) : IRequestHandler<SetProjectGroupCommand, CommandResult>
	{
		public async Task<CommandResult> Handle(SetProjectGroupCommand message, CancellationToken cancellationToken)
		{
			var validationResult = validator.Validate(message);
			if (!validationResult.IsValid)
				return new CommandValidationErrorResult(validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));

			var projectGroup = await projectGroupRepository.GetById(message.Urn);

			if (projectGroup == null)
			{
				_logger.LogError($"Project group is not found with ref:{message.Urn}");
				return new NotFoundCommandResult();
			}

			projectGroup.SetProjectGroup(message.TrustReference, dateTimeProvider.Now);

			projectGroupRepository.Update(projectGroup);
			await projectGroupRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
