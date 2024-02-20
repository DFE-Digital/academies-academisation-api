using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "AdvisoryBoardDecisionWithdrawnReasonState")]
public class AdvisoryBoardDecisionWithdrawnReasonState : BaseEntity
{
	public int AdvisoryBoardDecisionId { get; set; }
	public AdvisoryBoardWithdrawnReason Reason { get; init; }
	public string Details { get; init; } = null!;
}
