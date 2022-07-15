using System.ComponentModel.DataAnnotations.Schema;

using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;

namespace Dfe.Academies.Academisation.Data;

[Table(name: "application", Schema = "acad")]
public class ConversionApplicationState
{
	public ConversionApplicationState(IConversionApplication conversionApplication)
	{
		ApplicationType = conversionApplication.ApplicationType;
		Contributors = conversionApplication.Contributors
			.Select(contributor => new ContributorState(contributor.Details))
			.ToHashSet();
	}

	public int ConversionApplicationId { get; set; }
	public ApplicationType ApplicationType { get; set; }
	
	public HashSet<ContributorState> Contributors { get; set; }
}

