using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionApplicationAggregate;

[Table(name: "ConversionApplication", Schema = "academisation")]
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
}
