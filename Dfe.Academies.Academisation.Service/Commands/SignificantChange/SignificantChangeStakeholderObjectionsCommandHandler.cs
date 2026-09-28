using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public class SetSignificantChangeStakeholderObjectionsCommandHandler(ISignificantChangeProjectRepository repository, ILogger<SetSignificantChangeStakeholderObjectionsCommandHandler> logger) : IRequestHandler<SetSignificantChangeStakeholderObjectionsCommand, CommandResult>
{
	private readonly ISignificantChangeProjectRepository _repository = repository;
	private readonly ILogger<SetSignificantChangeStakeholderObjectionsCommandHandler> _logger = logger;

	public async Task<CommandResult> Handle(SetSignificantChangeStakeholderObjectionsCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await _repository.GetSignificantChangeProjectById(request.Id, cancellationToken);

		if (existingProject is null)
		{
			_logger.LogError("Significant change project not found with id: {ProjectId}", request.Id);
			return new NotFoundCommandResult();
		}

		existingProject.SetStakeholderObjections(
			request.StakeholderObjections,
			request.StakeholderObjectionsComment);

		_repository.Update(existingProject);
		await _repository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

		return new CommandSuccessResult();
	}
}