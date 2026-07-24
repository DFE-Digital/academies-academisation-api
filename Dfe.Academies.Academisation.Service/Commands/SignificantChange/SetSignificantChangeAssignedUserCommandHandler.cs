using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeAssignedUserCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeAssignedUserCommandHandler> logger) : IRequestHandler<SetSignificantChangeAssignedUserCommand, CommandResult>
{
	private readonly ISignificantChangeProjectRepository _repository = repository;
	private readonly ILogger<SetSignificantChangeAssignedUserCommandHandler> _logger = logger;

	public async Task<CommandResult> Handle(SetSignificantChangeAssignedUserCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError($"Significant change project not found with id: {request.Id}");
			return new NotFoundCommandResult();
		}

		existingProject.AssignUser(request.UserId, request.EmailAddress, request.FullName);

		_repository.Update(existingProject);
		await _repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}
