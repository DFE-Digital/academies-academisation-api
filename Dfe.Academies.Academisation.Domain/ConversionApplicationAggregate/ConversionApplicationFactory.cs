using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate
{
	public class ConversionApplicationFactory : IConversionApplicationFactory
	{
		public IConversionApplication Create(ApplicationType applicationType, IContributorDetails initialContributor)
		{
			// ToDo: Validate

			return new ConversionApplication(applicationType, initialContributor);
		}
	}
}
