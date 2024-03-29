﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferringAcademySchoolAdditionalDataCommandHandler : IRequestHandler<SetTransferringAcademySchoolAdditionalDataCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferringAcademySchoolAdditionalDataCommandHandler> _logger;

		public SetTransferringAcademySchoolAdditionalDataCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferringAcademySchoolAdditionalDataCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferringAcademySchoolAdditionalDataCommand request, CancellationToken cancellationToken)
		{
			
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);
			if (transferProject == null)
			{
				_logger.LogError($"transfer project not found with id:{request.Urn}");
				return new NotFoundCommandResult();
			}
			transferProject.SetTransferringAcademiesSchoolData(request.TransferringAcademyUkprn, request.LatestOfstedReportAdditionalInformation, request.PupilNumbersAdditionalInformation, request.KeyStage2PerformanceAdditionalInformation, request.KeyStage4PerformanceAdditionalInformation, request.KeyStage5PerformanceAdditionalInformation);
			_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject); 
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
