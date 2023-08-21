using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectLegalRequirementsCommandHandler : IRequestHandler<SetTransferProjectLegalRequirementsCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferProjectLegalRequirementsCommandHandler> _logger;

		public SetTransferProjectLegalRequirementsCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectLegalRequirementsCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferProjectLegalRequirementsCommand request, CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

			if (transferProject == null)
			{
				_logger.LogError($"transfer project not found with urn:{request.Urn}");
				return new NotFoundCommandResult();
			}
			transferProject.SetLegalRequirements(request.OutgoingTrustConsent, request.IncomingTrustAgreement, request.DiocesanConsent, request.IsCompleted);

			_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning this, client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
