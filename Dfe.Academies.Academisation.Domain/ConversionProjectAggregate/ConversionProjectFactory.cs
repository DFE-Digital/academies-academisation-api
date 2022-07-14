using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;

public class ConversionProjectFactory : IConversionProjectFactory
{
	public ConversionProjectFactory()
	{
	}

	public async Task<IConversionProject> Create(int projectId)
	{
		return await ConversionProject.Create(projectId);
	}
}