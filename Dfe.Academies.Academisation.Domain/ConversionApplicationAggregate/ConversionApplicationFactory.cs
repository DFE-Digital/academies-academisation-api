using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplicationFactory : IConversionApplicationFactory
{
	public async Task<CreateResult<IConversionApplication>> Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		return await ConversionApplication.Create(applicationType, initialContributor);
	}
}