using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

namespace Dfe.Academies.Academisation.Service;

public class ConversionProjectCreateCommand
{
	private readonly IConversionProjectFactory _factory;

	public ConversionProjectCreateCommand(IConversionProjectFactory factory)
	{
		_factory = factory;
	}

	public async Task<IConversionProject> Create(int projectId)
	{
		var conversionProject = await _factory.Create(projectId);

		//ToDo: Save to Database

		return conversionProject;
	}
}
