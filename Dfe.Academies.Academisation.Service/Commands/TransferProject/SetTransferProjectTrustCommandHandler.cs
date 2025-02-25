﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTrustCommandHandler : IRequestHandler<SetTransferProjectTrustCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferProjectTrustCommandHandler> _logger;

		public SetTransferProjectTrustCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectTrustCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferProjectTrustCommand request, CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);

			if (transferProject == null)
			{
				_logger.LogError($"transfer project not found with urn:{request.Urn}");
				return new NotFoundCommandResult();
			}

			foreach (var academy in transferProject?.TransferringAcademies)
			{
				transferProject.SetAcademyIncomingTrust(academy.Id, request.ProjectName, request.IncomingTrustReferenceNumber,request.IncomingTrustUKPRN);
			}

			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning this, client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
