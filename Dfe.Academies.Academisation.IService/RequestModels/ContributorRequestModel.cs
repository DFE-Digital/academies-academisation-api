using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels
{
	public record ContributorRequestModel(
		string FirstName,
		string LastName,
		string EmailAddress,
		ContributorRole Role,
		string? OtherRoleName);
}
