namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate
{
	public interface IConversionApplicationFactory
	{
		public IConversionApplication Create(ApplicationType applicationType, IContributorDetails initialContributor);
	}
}
