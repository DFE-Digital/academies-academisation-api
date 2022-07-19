using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;

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

	public async Task<ApplicationServiceModel> Create(ApplicationCreateRequestModel applicationCreateRequestModel)
	{
		var (applicationType, contributorDetails) = applicationCreateRequestModel.AsDomain();
		var application = await _domainFactory.Create(applicationType, contributorDetails);

		await _dataCommand.Execute(application);

		return ApplicationServiceModelMapper.FromDomain(application);
	}
}
