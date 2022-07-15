using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IContributor
{
	public int Id { get; }

	public ContributorDetails Details { get; }
}