using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;
using Dfe.Academies.Academisation.Core;

namespace Dfe.Academies.Academisation.Service;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IConversionApplicationFactory _domainFactory;
	private readonly IApplicationCreateDataCommand _dataCommand;

	public ApplicationCreateCommand(IConversionApplicationFactory domainFactory, IApplicationCreateDataCommand dataCommand)
	{
		_domainFactory = domainFactory;
		_dataCommand = dataCommand;
	}

	public async Task<ApplicationServiceModel> Execute(ApplicationCreateRequestModel applicationCreateRequestModel)
	{
		var (applicationType, contributorDetails) = applicationCreateRequestModel.AsDomain();
		var result = await _domainFactory.Create(applicationType, contributorDetails);

		if (result is CreateValidationErrorResult<IConversionApplication> domainValidationErrorResult)
		{
			throw new NotImplementedException();
		}

		if (result is not CreateSuccessResult<IConversionApplication> domainSuccessResult)
		{
			throw new NotImplementedException("Other CreateResult types not expected");
		}

		await _dataCommand.Execute(domainSuccessResult.Payload);

		return ApplicationServiceModelMapper.MapFromDomain(domainSuccessResult.Payload);
	}
}
