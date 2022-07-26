using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IConversionApplicationFactory
{
	CreateResult<IConversionApplication> Create(ApplicationType applicationType, ContributorDetails initialContributor);
}