using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.Commands;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IApplicationFactory _domainFactory;
	private readonly IApplicationCreateDataCommand _dataCommand;

	public ApplicationCreateCommand(IApplicationFactory domainFactory, IApplicationCreateDataCommand dataCommand)
	{
		_domainFactory = domainFactory;
		_dataCommand = dataCommand;
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

		return domainSuccessResult.MapToPayloadType(ApplicationServiceModelMapper.MapFromDomain);
	}
}
