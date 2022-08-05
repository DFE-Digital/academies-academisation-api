using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface IContributor
{
	public int Id { get; }

	public ContributorDetails Details { get; }
}