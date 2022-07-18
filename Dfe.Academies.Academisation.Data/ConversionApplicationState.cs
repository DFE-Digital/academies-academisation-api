using System.ComponentModel.DataAnnotations.Schema;

using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data;

[Table(name: "application", Schema = "acd")]
public class ConversionApplicationState : BaseEntity
{
	public ConversionApplicationState(IConversionApplication conversionApplication)
	{
		ApplicationType = conversionApplication.ApplicationType;
		Contributors = conversionApplication.Contributors
			.Select(contributor => new ConversionApplicationContributorState(contributor.Details))
			.ToHashSet();
	}

	public int ConversionApplicationId { get; set; }
	public ApplicationType ApplicationType { get; set; }
	
	public HashSet<ConversionApplicationContributorState> Contributors { get; set; }
}

