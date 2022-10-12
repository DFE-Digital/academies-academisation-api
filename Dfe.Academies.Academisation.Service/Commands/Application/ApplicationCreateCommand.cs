using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IApplicationFactory _domainFactory;
	private readonly IApplicationCreateDataCommand _dataCommand;
	private readonly IMapper _mapper;

	public ApplicationCreateCommand(IApplicationFactory domainFactory, IApplicationCreateDataCommand dataCommand, IMapper mapper)
	{
		_domainFactory = domainFactory;
		_dataCommand = dataCommand;
		_mapper = mapper;
	}

	public async Task<CreateResult<ApplicationServiceModel>> Execute(ApplicationCreateRequestModel applicationCreateRequestModel)
	{
		var (applicationType, contributorDetails) = applicationCreateRequestModel.AsDomain();
		var result = _domainFactory.Create(applicationType, contributorDetails);

		if (result is CreateValidationErrorResult<IApplication> domainValidationErrorResult)
		{
			return domainValidationErrorResult.MapToPayloadType<ApplicationServiceModel>();
		}

		if (result is not CreateSuccessResult<IApplication> domainSuccessResult)
		{
			throw new NotImplementedException("Other CreateResult types not expected");
		}

		await _dataCommand.Execute(domainSuccessResult.Payload);

		return domainSuccessResult.MapToPayloadType(ApplicationServiceModelMapper.MapFromDomain, _mapper);
	}
}
