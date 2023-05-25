using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels
{
	/// <summary>
	/// Represents a request model sent to the API.
	/// Note that this type is replicated in the Web API Contracts project
	/// </summary>
	public record ContributorRequestModel(
		string FirstName,
		string LastName,
		string EmailAddress,
		ContributorRole Role,
		string? OtherRoleName);
}
