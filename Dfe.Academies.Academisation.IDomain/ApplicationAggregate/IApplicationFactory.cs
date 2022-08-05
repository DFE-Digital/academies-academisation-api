using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface IApplicationFactory
{
	CreateResult<IApplication> Create(ApplicationType applicationType, ContributorDetails initialContributor);
}