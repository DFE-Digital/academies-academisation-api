using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "ConversionAdvisoryBoardDecision")]
public class ConversionAdvisoryBoardDecisionState : BaseEntity
{
    public int ConversionProjectId { get; set; }
    public AdvisoryBoardDecisions Decision { get; set; }
    public bool? ApprovedConditionsSet { get; set; }
    public string? ApprovedConditionsDetails { get; set; }
        
    [ForeignKey(nameof(ConversionAdvisoryBoardDecisionDeclinedReasonState.AdvisoryBoardDecisionId))]
    public HashSet<ConversionAdvisoryBoardDecisionDeclinedReasonState>? DeclinedReasons { get; set; }
    public string? DeclinedOtherReason { get; set; }
        
    [ForeignKey(nameof(ConversionAdvisoryBoardDecisionDeferredReasonState.AdvisoryBoardDecisionId))]
    public HashSet<ConversionAdvisoryBoardDecisionDeferredReasonState>? DeferredReasons { get; set; }
    public string? DeferredOtherReason { get; set; }
    public DateTime AdvisoryBoardDecisionDate { get; set; }
    public DecisionMadeBy DecisionMadeBy { get; set; }

    internal static ConversionAdvisoryBoardDecisionState MapFromDomain(IConversionAdvisoryBoardDecision decision)
    {
        return new()
        {
            ConversionProjectId = decision.AdvisoryBoardDecisionDetails.ConversionProjectId,
            Decision = decision.AdvisoryBoardDecisionDetails.Decision,
            ApprovedConditionsSet = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet,
            ApprovedConditionsDetails = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails,
            DeclinedReasons = decision.AdvisoryBoardDecisionDetails.DeclinedReasons?
                .Select(reason => new ConversionAdvisoryBoardDecisionDeclinedReasonState {Reason = reason})
                .ToHashSet(),
            DeclinedOtherReason = decision.AdvisoryBoardDecisionDetails.DeclinedOtherReason,
            DeferredReasons = decision.AdvisoryBoardDecisionDetails.DeferredReasons?
                .Select(reason => new ConversionAdvisoryBoardDecisionDeferredReasonState {Reason = reason})
                .ToHashSet(),
            DeferredOtherReason = decision.AdvisoryBoardDecisionDetails.DeferredOtherReason,
            AdvisoryBoardDecisionDate = decision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate,
            DecisionMadeBy = decision.AdvisoryBoardDecisionDetails.DecisionMadeBy
        };
    }
}
