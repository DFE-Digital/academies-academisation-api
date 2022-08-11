using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "ConversionAdvisoryBoardDecisionDeclinedReason")]
public class ConversionAdvisoryBoardDecisionDeclinedReasonState : BaseEntity
{
	public int AdvisoryBoardDecisionId { get; set; }
	public AdvisoryBoardDeclinedReason Reason { get; init; }
	public string Details { get; init; } = null!;
}
