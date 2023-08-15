using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferProjectTrustInformationAndProjectDatesCommandHandler : IRequestHandler<SetTransferProjectTrustInformationAndProjectDatesCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferProjectTrustInformationAndProjectDatesCommandHandler> _logger;

		public SetTransferProjectTrustInformationAndProjectDatesCommandHandler(ITransferProjectRepository transferProjectRepository,
			ILogger<SetTransferProjectTrustInformationAndProjectDatesCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferProjectTrustInformationAndProjectDatesCommand request,
			CancellationToken cancellationToken)
		{
			var transferProject = await _transferProjectRepository.GetById(request.Id).ConfigureAwait(false);

			if (transferProject == null)
			{
				_logger.LogError($"transfer project not found with id:{request.Id}");
				return new NotFoundCommandResult();
			}

			transferProject.SetTrustInformationAndProjectDates(request.Recommendation, request.Author);

			_transferProjectRepository.Update(transferProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
