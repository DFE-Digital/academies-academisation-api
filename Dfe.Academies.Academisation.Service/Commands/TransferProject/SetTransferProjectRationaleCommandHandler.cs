using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using MediatR;
using Microsoft.Extensions.Logging;
using TramsDataApi.RequestModels.AcademyTransferProject;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class SetTransferProjectRationaleCommandHandler : IRequestHandler<SetTransferProjectRationaleCommand, CommandResult>
{
	private readonly ITransferProjectRepository _transferProjectRepository;
	private readonly ILogger<SetTransferProjectRationaleCommandHandler> _logger;

	public SetTransferProjectRationaleCommandHandler(ITransferProjectRepository transferProjectRepository, ILogger<SetTransferProjectRationaleCommandHandler> logger)
	{
		_transferProjectRepository = transferProjectRepository;
		_logger = logger;
	}

	public async Task<CommandResult> Handle(SetTransferProjectRationaleCommand request, CancellationToken cancellationToken)
	{
		var transferProject = await _transferProjectRepository.GetById(request.Id).ConfigureAwait(false);

		if (transferProject == null) {
			_logger.LogError($"transfer project not found with id:{request.Id}");
			return new NotFoundCommandResult();
		}
		transferProject.SetRationale(request.ProjectRationale, request.TrustSponsorRationale, request.IsCompleted);

		 _transferProjectRepository.Update(transferProject);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		// returnin this, client will have to retrieve the updated transfer pproject to refresh data
		return new CommandSuccessResult();
	}
}
