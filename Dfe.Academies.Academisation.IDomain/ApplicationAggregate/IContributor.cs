using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface IContributor
{
	public int Id { get; }

	public ContributorDetails Details { get; }

	void Update(ContributorDetails details);
}
