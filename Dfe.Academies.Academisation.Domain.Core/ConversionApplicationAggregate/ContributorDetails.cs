namespace Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;

public record ContributorDetails
(
	string FirstName,
	string LastName,
	string EmailAddress,
	ContributorRole Role,
	string? OtherRoleName
);
