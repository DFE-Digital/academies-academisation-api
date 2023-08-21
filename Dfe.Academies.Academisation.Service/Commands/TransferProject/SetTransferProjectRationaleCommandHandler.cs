using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class
	SetTransferProjectRationaleCommandHandler : IRequestHandler<SetTransferProjectRationaleCommand, CommandResult>
{
	private readonly ITransferProjectRepository _transferProjectRepository;
	private readonly ILogger<SetTransferProjectRationaleCommandHandler> _logger;

	public SetTransferProjectRationaleCommandHandler(ITransferProjectRepository transferProjectRepository,
		ILogger<SetTransferProjectRationaleCommandHandler> logger)
	{
		_transferProjectRepository = transferProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetTransferProjectRationaleCommand request,
		CancellationToken cancellationToken)
	{
		var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

		if (transferProject == null)
		{
			_logger.LogError($"transfer project not found with urn:{request.Urn}");
			return new NotFoundCommandResult();
		}

		transferProject.SetRationale(request.ProjectRationale, request.TrustSponsorRationale, request.IsCompleted);

		_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
		return new CommandSuccessResult();
	}
}

