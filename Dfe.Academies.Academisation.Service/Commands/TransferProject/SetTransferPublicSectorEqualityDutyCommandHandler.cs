using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferPublicSectorEqualityDutyCommandHandler
	: IRequestHandler<SetTransferPublicSectorEqualityDutyCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferPublicSectorEqualityDutyCommandHandler> _logger;

		public SetTransferPublicSectorEqualityDutyCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferPublicSectorEqualityDutyCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferPublicSectorEqualityDutyCommand request, CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

			if (transferProject == null)
			{
				_logger.LogError("transfer project not found with urn:{Urn}", request.Urn);
				return new NotFoundCommandResult();
			}

			transferProject.SetPublicSectorEqualityDuty(request.HowLikelyImpactProtectedCharacteristics,
											   request.WhatWillBeDoneToReduceImpact!,
											   request.IsCompleted);

			_transferProjectRepository.Update((transferProject as Domain.TransferProjectAggregate.TransferProject)!);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning this, client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
