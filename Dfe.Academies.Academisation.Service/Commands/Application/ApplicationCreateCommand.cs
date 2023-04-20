using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IApplicationFactory _domainFactory;
	private readonly IApplicationRepository _applicationRepository;
	private readonly IMapper _mapper;

	public ApplicationCreateCommand(IApplicationFactory domainFactory, IApplicationRepository applicationRepository, IMapper mapper)
	{
		_domainFactory = domainFactory;
		_applicationRepository = applicationRepository;
		_mapper = mapper;
	}

	public async Task<CreateResult> Execute(ApplicationCreateRequestModel applicationCreateRequestModel)
	{
		(ApplicationType applicationType, ContributorDetails contributorDetails) = applicationCreateRequestModel.AsDomain();
		var result = _domainFactory.Create(applicationType, contributorDetails);

		if (result is CreateValidationErrorResult domainValidationErrorResult)
		{
			return domainValidationErrorResult.MapToPayloadType();
		}

		if (result is not CreateSuccessResult<Domain.ApplicationAggregate.Application> domainSuccessResult)
		{
			throw new NotImplementedException("Other CreateResult types not expected");
		}

		await _applicationRepository.Insert(domainSuccessResult.Payload);
		await _applicationRepository.UnitOfWork.SaveChangesAsync();

		return domainSuccessResult.MapToPayloadType(ApplicationServiceModelMapper.MapFromDomain, _mapper);
	}
}
