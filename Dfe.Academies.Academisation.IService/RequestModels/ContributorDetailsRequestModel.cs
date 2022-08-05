using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels
{
	public class ContributorDetailsRequestModel
	{
		public ContributorDetailsRequestModel(string firstName, string lastName, string emailAddress, ContributorRole role, string? otherRoleName)
		{
			FirstName = firstName;
			LastName = lastName;
			EmailAddress = emailAddress;
			Role = role;
			OtherRoleName = otherRoleName;
		}

		public string FirstName { get; }
		public string LastName { get; }
		public string EmailAddress { get; }
		public ContributorRole Role { get; }
		public string? OtherRoleName { get; }
	}
}
