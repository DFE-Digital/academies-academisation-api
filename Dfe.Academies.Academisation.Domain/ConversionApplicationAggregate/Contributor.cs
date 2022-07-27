using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;

public class Contributor : IContributor
{
	internal Contributor(ContributorDetails details)
	{
		Details = details;
	}
	internal Contributor(int id, ContributorDetails details)
	{
		Id = id;
		Details = details;
	}

	public int Id { get; internal set; }

	public ContributorDetails Details { get; }
}
