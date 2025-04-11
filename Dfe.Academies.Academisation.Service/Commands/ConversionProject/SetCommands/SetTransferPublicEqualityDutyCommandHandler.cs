using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands
{
	public class SetTransferPublicEqualityDutyCommandHandler : IRequestHandler<SetTransferPublicEqualityDutyCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferPublicEqualityDutyCommandHandler> _logger;

		public SetTransferPublicEqualityDutyCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferPublicEqualityDutyCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferPublicEqualityDutyCommand request, CancellationToken cancellationToken)
		{
			var existingProject = await _transferProjectRepository.GetTransferProjectById(request.Urn, cancellationToken);

			if (existingProject is null)
			{
				_logger.LogError($"transfer project not found with Urn:{request.Urn}");
				return new NotFoundCommandResult();
			}

			existingProject.SetPublicEqualityDuty(request.PublicEqualityDutyImpact, request.PublicEqualityDutyReduceImpactReason, request.PublicEqualityDutySectionComplete);

			_transferProjectRepository.Update(existingProject as Domain.TransferProjectAggregate.TransferProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}
