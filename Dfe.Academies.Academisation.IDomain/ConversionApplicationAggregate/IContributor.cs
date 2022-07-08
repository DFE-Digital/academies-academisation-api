namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

public interface IContributor
{
	public int Id { get; }

	public IContributorDetails Details { get; }
}