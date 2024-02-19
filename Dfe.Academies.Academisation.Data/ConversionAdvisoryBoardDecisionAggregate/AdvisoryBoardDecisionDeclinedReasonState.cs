using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "AdvisoryBoardDecisionDeclinedReason")]
public class AdvisoryBoardDecisionDeclinedReasonState : BaseEntity
{
	public int AdvisoryBoardDecisionId { get; set; }
	public AdvisoryBoardDeclinedReason Reason { get; init; }
	public string Details { get; init; } = null!;
}
