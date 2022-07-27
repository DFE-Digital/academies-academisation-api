using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;
using Dfe.Academies.Academisation.Core;

namespace Dfe.Academies.Academisation.Service.Commands;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IConversionApplicationFactory _domainFactory;
	private readonly IApplicationCreateDataCommand _dataCommand;

	public ApplicationCreateCommand(IConversionApplicationFactory domainFactory, IApplicationCreateDataCommand dataCommand)
	{
		_domainFactory = domainFactory;
		_dataCommand = dataCommand;
	}

	public async Task<CreateResult<ApplicationServiceModel>> Execute(ApplicationCreateRequestModel applicationCreateRequestModel)
	{
		var (applicationType, contributorDetails) = applicationCreateRequestModel.AsDomain();
		var result = _domainFactory.Create(applicationType, contributorDetails);

		if (result is CreateValidationErrorResult<IConversionApplication> domainValidationErrorResult)
		{
			return domainValidationErrorResult.MapToPayloadType<ApplicationServiceModel>();
		}

		if (result is not CreateSuccessResult<IConversionApplication> domainSuccessResult)
		{
			throw new NotImplementedException("Other CreateResult types not expected");
		}

		await _dataCommand.Execute(domainSuccessResult.Payload);

		return domainSuccessResult.MapToPayloadType(ApplicationServiceModelMapper.MapFromDomain);
	}
}
