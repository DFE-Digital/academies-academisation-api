using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service;

public class ApplicationCreateCommand : IApplicationCreateCommand
{
	private readonly IConversionApplicationFactory _factory;

	public ApplicationCreateCommand(IConversionApplicationFactory factory)
	{
		_factory = factory;
	}

	public async Task<ApplicationServiceModel> Create(ApplicationCreateRequestModel conversionApplicationRequestModel)
	{
		IConversionApplication application = await _factory.Create((ApplicationType) conversionApplicationRequestModel.ApplicationType, null);

		// ToDo: Save to Database

		return new();
	}
}
