using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate
{
	public class Contributor : IContributor
	{
		public Contributor(IContributorDetails details)
		{
			Details = details;
		}

		public int Id { get; }

		public IContributorDetails Details { get; internal set; }
	}
}
