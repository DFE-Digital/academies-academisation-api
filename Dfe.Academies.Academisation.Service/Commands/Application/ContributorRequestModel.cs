using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public record ContributorRequestModel(
		string FirstName,
		string LastName,
		string EmailAddress,
		ContributorRole Role,
		string? OtherRoleName);
}
