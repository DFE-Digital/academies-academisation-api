using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.TransferProject
{
	public class SetTransferSfsoCommissioningCommandHandler : IRequestHandler<SetTransferSfsoCommissioningCommand, CommandResult>
	{
		private const int MaxOverviewLength = 250;
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly ILogger<SetTransferSfsoCommissioningCommandHandler> _logger;

		public SetTransferSfsoCommissioningCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferSfsoCommissioningCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(SetTransferSfsoCommissioningCommand request, CancellationToken cancellationToken)
		{
			if (request.SfsoCommissioningOverview?.Length > MaxOverviewLength)
			{
				return new CommandValidationErrorResult(new List<ValidationError>
				{
					new("SfsoCommissioningOverview", $"Overview must be {MaxOverviewLength} characters or less")
				});
			}

			var existingProject = await _transferProjectRepository.GetByUrn(request.Urn);
			if (existingProject is null)
			{
				_logger.LogError("transfer project not found with Urn:{Urn}", request.Urn);
				return new NotFoundCommandResult();
			}

			existingProject.SetSfsoCommissioning(request.SfsoCommissioningOverview);
			_transferProjectRepository.Update((Domain.TransferProjectAggregate.TransferProject)existingProject);
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

			return new CommandSuccessResult();
		}
	}
}