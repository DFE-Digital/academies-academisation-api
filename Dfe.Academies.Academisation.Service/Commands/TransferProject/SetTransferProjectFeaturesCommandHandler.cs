using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectFeaturesCommandHandler : IRequestHandler<SetTransferProjectFeaturesCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferProjectFeaturesCommandHandler> _logger;

		public SetTransferProjectFeaturesCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectFeaturesCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferProjectFeaturesCommand request, CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

			if (transferProject == null)
			{
				_logger.LogError($"transfer project not found with urn:{request.Urn}");
				return new NotFoundCommandResult();
			}
			transferProject.SetFeatures(request.WhoInitiatedTheTransfer, request.TypeOfTransfer, request.IsCompleted);

			_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning this, client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
