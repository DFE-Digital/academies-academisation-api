using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Academies;
using Dfe.Academies.Contracts.V4.Trusts;
using MediatR;
using Microsoft.Extensions.Logging;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class PopulateTrustNamesCommandHandler : IRequestHandler<PopulateTrustNamesCommand, CommandResult>
	{
		private readonly ITransferProjectRepository _transferProjectRepository;
		private readonly IAcademiesQueryService _academiesQueryService;
		private readonly ILogger<AssignTransferProjectUserCommandHandler> _logger;

		public PopulateTrustNamesCommandHandler(ITransferProjectRepository transferProjectRepository, IAcademiesQueryService academiesQueryService,
			ILogger<AssignTransferProjectUserCommandHandler> logger)
		{
			_transferProjectRepository = transferProjectRepository;
			_academiesQueryService = academiesQueryService;
			_logger = logger;
		}

		public async Task<CommandResult> Handle(PopulateTrustNamesCommand request,
			CancellationToken cancellationToken)
		{
			var transferProjects = await _transferProjectRepository.GetAllTransferProjectsWhereTrustNameIsNull().ConfigureAwait(false);

			if (transferProjects == null || !transferProjects.Any())
			{
				_logger.LogError($"No trnasfer projects found with empty trust names");
				return new NotFoundCommandResult();
			}

			foreach (var transferProject in transferProjects)
			{
				var outgoingTrust = await _academiesQueryService.GetTrust(transferProject.OutgoingTrustUkprn).ConfigureAwait(false);

				if (outgoingTrust != null) { transferProject.SetOutgoingTrustName(outgoingTrust?.Name);  }

				TrustDto? incomingTrust = null;

				foreach (var academy in transferProject.TransferringAcademies)
				{
					if (incomingTrust == null || incomingTrust.Ukprn != academy.IncomingTrustUkprn)
					{
						incomingTrust = await _academiesQueryService.GetTrust(academy.IncomingTrustUkprn).ConfigureAwait(false);
					}

					if (incomingTrust != null) { transferProject.SetAcademyIncomingTrustName(academy.Id, incomingTrust?.Name); }
				}
				_transferProjectRepository.Update(transferProject as Domain.TransferProjectAggregate.TransferProject);
			}

			
			await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			// returning 'CommandSuccessResult', client will have to retrieve the updated transfer project to refresh data
			return new CommandSuccessResult();
		}
	}
}
