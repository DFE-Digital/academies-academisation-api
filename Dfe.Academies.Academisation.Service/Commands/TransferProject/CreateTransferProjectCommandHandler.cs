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

	public async Task<CreateResult> Handle(CreateTransferProjectCommand message, CancellationToken cancellationToken)
	{
		List<TransferringAcademy> transferringAcademies = new List<TransferringAcademy>();

		foreach (var transferringAcademy in message.TransferringAcademies)
		{
			transferringAcademies.Add(new TransferringAcademy(transferringAcademy.IncomingTrustUkprn, transferringAcademy.IncomingTrustName, transferringAcademy.OutgoingAcademyUkprn, transferringAcademy.Region, transferringAcademy.LocalAuthority));
		}

		var transferProject = Domain.TransferProjectAggregate.TransferProject.Create(
			message.OutgoingTrustUkprn, 
			message.OutgoingTrustName,
			transferringAcademies,
			message.IsFormAMat,
			_dateTimeProvider.Now);

		 _transferProjectRepository.Insert(transferProject);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		transferProject.GenerateUrn();

		if (!string.IsNullOrEmpty(message.Reference))
		{
			transferProject.GenerateReference(message.Reference);
		}

		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return new CreateSuccessResult<AcademyTransferProjectResponse>(AcademyTransferProjectResponseFactory.Create(transferProject));
	}
}
