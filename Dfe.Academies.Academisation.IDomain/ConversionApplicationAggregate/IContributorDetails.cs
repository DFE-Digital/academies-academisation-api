namespace Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate
{
	public interface IContributorDetails
	{
		string FirstName { get; }

		string LastName { get; }

		string EmailAddress { get; }

		ContributorRole Role { get; }

		string? OtherRoleName { get; }
	}
}
