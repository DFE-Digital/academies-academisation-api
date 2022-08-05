using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

[Table(name: "ConversionApplicationContributor")]
public class ContributorState : BaseEntity
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string EmailAddress { get; set; } = null!;
	public ContributorRole Role { get; set; }
	public string? OtherRoleName { get; set; }
	
	public static ContributorState MapFromDomain(IContributor contributor)
	{
		return new()
		{
			Id = contributor.Id,
			FirstName = contributor.Details.FirstName,
			LastName = contributor.Details.LastName,
			EmailAddress = contributor.Details.EmailAddress,
			Role = contributor.Details.Role,
			OtherRoleName = contributor.Details.OtherRoleName
		};
	}
}
