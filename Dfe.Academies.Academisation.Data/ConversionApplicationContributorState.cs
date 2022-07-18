using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.Data;

[Table(name: "Contributor", Schema = "acd")]
public class ConversionApplicationContributorState : BaseEntity
{
	public ConversionApplicationContributorState(ContributorDetails details)
	{
		(FirstName, LastName, EmailAddress, Role, OtherRoleName) = details;
	}
	
	public int ContributorId { get; set; }
	public int ApplicationId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string EmailAddress { get; set; }
	public ContributorRole Role { get; set; }
	public string? OtherRoleName { get; set; }

	
}
