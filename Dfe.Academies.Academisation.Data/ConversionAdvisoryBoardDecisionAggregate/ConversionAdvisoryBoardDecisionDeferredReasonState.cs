using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "ConversionAdvisoryBoardDecisionDeferredReason")]
public class ConversionAdvisoryBoardDecisionDeferredReasonState : BaseEntity
{
	public int AdvisoryBoardDecisionId { get; set; }
	public AdvisoryBoardDeferredReason Reason { get; init; }
	public string Details { get; init; } = null!;
}
