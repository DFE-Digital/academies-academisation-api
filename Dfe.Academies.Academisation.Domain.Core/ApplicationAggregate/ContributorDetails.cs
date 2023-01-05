namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

public class ContributorDetails
{
	public ContributorDetails(string FirstName,
		string LastName,
		string EmailAddress,
		ContributorRole Role,
		string? OtherRoleName)
	{
		this.FirstName = FirstName;
		this.LastName = LastName;
		this.EmailAddress = EmailAddress;
		this.Role = Role;
		this.OtherRoleName = OtherRoleName;;
	}

	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string EmailAddress { get; init; }
	public ContributorRole Role { get; init; }
	public string? OtherRoleName { get; init; }

	public void Deconstruct(out string FirstName, out string LastName, out string EmailAddress, out ContributorRole Role, out string? OtherRoleName)
	{
		FirstName = this.FirstName;
		LastName = this.LastName;
		EmailAddress = this.EmailAddress;
		Role = this.Role;
		OtherRoleName = this.OtherRoleName;;
	}
}
