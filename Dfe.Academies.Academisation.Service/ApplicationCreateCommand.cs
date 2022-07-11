using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Service;

public class ApplicationCreateCommand
{
	private readonly IConversionApplicationFactory _factory;

	public ApplicationCreateCommand(IConversionApplicationFactory factory)
	{
		_factory = factory;
	}

	public async Task<IConversionApplication> Create(ApplicationType applicationType, IContributorDetails initialContributor)
	{
		IConversionApplication application = await _factory.Create(applicationType, initialContributor);

		// ToDo: Save to Database

		return application;
	}
}
