using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferringAcademyGeneralInformationCommandHandler : IRequestHandler<SetTransferringAcademyGeneralInformationCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferringAcademyGeneralInformationCommandHandler> _logger;

		public SetTransferringAcademyGeneralInformationCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferringAcademyGeneralInformationCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferringAcademyGeneralInformationCommand request, CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetByUrn(request.Urn).ConfigureAwait(false);
			if (transferProject == null)
			{
				_logger.LogError($"Transfer project not found with id: {request.Urn}");
				return new NotFoundCommandResult();
			}

			transferProject.SetTransferringAcademyGeneralInformation(request.TransferringAcademyUkprn, request.PFIScheme, request.PFISchemeDetails, request.DistanceFromAcademyToTrustHq, request.DistanceFromAcademyToTrustHqDetails, request.ViabilityIssues, request.FinancialDeficit, request.MPNameAndParty, request.PublishedAdmissionNumber);
			_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CommandSuccessResult();
		}
	}
}
