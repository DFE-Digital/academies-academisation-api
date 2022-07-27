using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class ConversionApplicationFactory : IConversionApplicationFactory
{
	public CreateResult<IConversionApplication> Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		return ConversionApplication.Create(applicationType, initialContributor);
	}
}