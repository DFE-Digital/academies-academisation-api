using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class SetTransferProjectDeletedAtCommandHandler : IRequestHandler<SetTransferProjectDeletedAtCommand, CommandResult>
{
	private readonly ITransferProjectRepository _transferProjectRepository;
	private readonly ILogger<SetTransferProjectDeletedAtCommandHandler> _logger;

	public SetTransferProjectDeletedAtCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectDeletedAtCommandHandler> logger)
	{
		_transferProjectRepository = transferProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetTransferProjectDeletedAtCommand request, CancellationToken cancellationToken)
	{
		var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

		if (transferProject == null) {
			_logger.LogError($"transfer project not found with urn:{request.Urn}");
			return new NotFoundCommandResult();
		}
		transferProject.SetDeletedAt();

		 _transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
		return new CommandSuccessResult();
	}
}
