using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class ApplicationFactory : IApplicationFactory
{
	public CreateResult Create(ApplicationType applicationType,
		ContributorDetails initialContributor)
	{
		return Application.Create(applicationType, initialContributor);
	}
}
