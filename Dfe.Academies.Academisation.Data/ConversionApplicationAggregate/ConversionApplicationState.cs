using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

[Table(name: "ConversionApplication")]
public class ConversionApplicationState : BaseEntity
{
	public ApplicationType ApplicationType { get; set; }

	[ForeignKey("ConversionApplicationId")]
	public HashSet<ContributorState> Contributors { get; set; } = null!;

	public static ConversionApplicationState MapFromDomain(IConversionApplication conversionApplication)
	{
		return new()
		{
			ApplicationType = conversionApplication.ApplicationType,
			Contributors = conversionApplication.Contributors
				.Select(ContributorState.MapFromDomain)
				.ToHashSet()
		};
	}

	public IConversionApplication MapToDomain()
	{
		var contributorsDictionary = Contributors.ToDictionary(
			c => c.Id,
			c => new ContributorDetails(c.FirstName, c.LastName, c.EmailAddress, c.Role, c.OtherRoleName));

		return new ConversionApplication(Id, ApplicationType, contributorsDictionary);
	}
}
