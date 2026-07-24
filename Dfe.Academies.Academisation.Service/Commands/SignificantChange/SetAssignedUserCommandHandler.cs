using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetAssignedUserCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetAssignedUserCommandHandler> logger) : IRequestHandler<SetAssignedUserCommand, CommandResult>
{
	private readonly ISignificantChangeProjectRepository _repository = repository;
	private readonly ILogger<SetAssignedUserCommandHandler> _logger = logger;

	public async Task<CommandResult> Handle(SetAssignedUserCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _repository.GetSignificantChangeProject(request.Id, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError($"Significant change project not found with id: {request.Id}");
			return new NotFoundCommandResult();
		}

		// Update the school overview information in the existing project
		existingProject.SetAssignedUser(request.UserId, request.FullName, request.EmailAddress);

		_repository.Update(existingProject as Project);
		await _repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
