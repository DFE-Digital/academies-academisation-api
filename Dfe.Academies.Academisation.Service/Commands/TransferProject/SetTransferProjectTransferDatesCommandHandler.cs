﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTransferDatesCommandHandler : IRequestHandler<SetTransferProjectTransferDatesCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferProjectTransferDatesCommandHandler> _logger;

		public SetTransferProjectTransferDatesCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectTransferDatesCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferProjectTransferDatesCommand request, CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

			if (transferProject == null)
			{
				_logger.LogError($"transfer project not found with Urn:{request.Urn}");
				return new NotFoundCommandResult();
			}

			transferProject.SetTransferDates(request.HtbDate, request.PreviousAdvisoryBoardDate, request.TargetDateForTransfer, request.IsCompleted, request.ChangedBy, request.ReasonsChanged);

			_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning this, client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
