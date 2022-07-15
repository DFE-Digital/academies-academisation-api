using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplicationFactory : IConversionApplicationFactory
{
	public async Task<IConversionApplication> Create(ApplicationType applicationType,
		IContributorDetails initialContributor)
	{
		return await ConversionApplication.Create(applicationType, initialContributor);
	}
}