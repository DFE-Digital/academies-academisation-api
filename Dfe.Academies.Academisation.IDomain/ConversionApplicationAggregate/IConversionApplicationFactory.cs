using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IConversionApplicationFactory
{
	Task<IConversionApplication> Create(ApplicationType applicationType, ContributorDetails initialContributor);
}