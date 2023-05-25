namespace Dfe.Academies.Academisation.WebApi.Contracts.RequestModels;
using Dfe.Academies.Academisation.WebApi.Contracts.FromDomain;

public record ContributorRequestModel(
	string FirstName,
	string LastName,
	string EmailAddress,
	ContributorRole Role,
	string? OtherRoleName);
