using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Dfe.Academies.Academisation.Service.Mappers.Application;
using Dfe.Academies.Academisation.Service.Mappers.TransferProject;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class CreateTransferProjectCommandHandler : IRequestHandler<CreateTransferProjectCommand, CreateResult>
{
	private readonly ITransferProjectRepository _transferProjectRepository;
	private readonly IMapper _mapper;
	private readonly IDateTimeProvider _dateTimeProvider;

	public CreateTransferProjectCommandHandler(ITransferProjectRepository transferProjectRepository, IMapper mapper, IDateTimeProvider dateTimeProvider)
	{
		_transferProjectRepository = transferProjectRepository;
		_mapper = mapper;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<CreateResult> Handle(CreateTransferProjectCommand request, CancellationToken cancellationToken)
	{
		var result = Domain.TransferProjectAggregate.TransferProject.Create(request.OutgoingTrustUkprn, request.IncomingTrustUkprn, request.TransferringAcademyUkprns, _dateTimeProvider.Now);

		 _transferProjectRepository.Insert(result);
		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		result.GenerateUrn();

		await _transferProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return new CreateSuccessResult<AcademyTransferProjectResponse>(AcademyTransferProjectResponseFactory.Create(result));
	}
}
