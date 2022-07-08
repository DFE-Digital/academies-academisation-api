using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate
{
	public record ContributorDetails
	(
		string FirstName,
		string LastName,
		string EmailAddress,
		ContributorRole Role,
		string? RoleOther
	) : IContributorDetails;
}
