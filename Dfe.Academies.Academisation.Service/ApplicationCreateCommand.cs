using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;

namespace Dfe.Academies.Academisation.Service;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IConversionApplicationFactory _factory;

	public ApplicationCreateCommand(IConversionApplicationFactory factory)
	{
		_factory = factory;
	}

	public async Task<ApplicationServiceModel> Create(ApplicationCreateRequestModel applicationCreateRequestModel)
	{
		var (applicationType, contributorDetails) = applicationCreateRequestModel.AsDomain();
		IConversionApplication application = await _factory.Create(applicationType, contributorDetails);

		// ToDo: Save to Database

		return new();
	}
}
