namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate
{
	public interface IConversionApplicationFactory
	{
		Task<IConversionApplication> Create(ApplicationType applicationType, IContributorDetails initialContributor);
	}
}
