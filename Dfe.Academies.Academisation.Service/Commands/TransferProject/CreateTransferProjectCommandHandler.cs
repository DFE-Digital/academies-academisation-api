using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class CreateTransferProjectCommandHandler : IRequestHandler<CreateTransferProjectCommand, CreateResult>
{
	private readonly ITransferProjectRepository _transferProjectRepository;
	// need to use this in place of the AcademyTransferProjectResponseFactory but for now use this to reduce chance of introducing bugs
	private readonly IDateTimeProvider _dateTimeProvider;

	public CreateTransferProjectCommandHandler(ITransferProjectRepository transferProjectRepository, IDateTimeProvider dateTimeProvider)
	{
		_transferProjectRepository = transferProjectRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<CreateResult> Handle(CreateTransferProjectCommand request, CancellationToken cancellationToken)
	{
		var transferProject = Domain.TransferProjectAggregate.TransferProject.Create(
			request.OutgoingTrustUkprn, 
			request.OutgoingTrustName,
			request.IncomingTrustUkprn,
			request.IncomingTrustName,
			request.TransferringAcademyUkprns, 
			request.IsFormAMat,
			_dateTimeProvider.Now);

		 _transferProjectRepository.Insert(transferProject);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		transferProject.GenerateUrn();

		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return new CreateSuccessResult<AcademyTransferProjectResponse>(AcademyTransferProjectResponseFactory.Create(transferProject));
	}
}
