using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class Contributor : IContributor
{
	public Contributor(ContributorDetails details)
	{
		Details = details;
	}

	public int Id { get; }

	public ContributorDetails Details { get; }
}