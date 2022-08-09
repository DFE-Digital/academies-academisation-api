using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "ConversionAdvisoryBoardDecision")]
public class ConversionAdvisoryBoardDecisionState : BaseEntity
{
    public int ConversionProjectId { get; init; }
    public AdvisoryBoardDecision Decision { get; init; }
    public bool? ApprovedConditionsSet { get; init; }
    public string? ApprovedConditionsDetails { get; init; }
        
    [ForeignKey(nameof(ConversionAdvisoryBoardDecisionDeclinedReasonState.AdvisoryBoardDecisionId))]
    public HashSet<ConversionAdvisoryBoardDecisionDeclinedReasonState>? DeclinedReasons { get; init; }
    public string? DeclinedOtherReason { get; init; }
        
    [ForeignKey(nameof(ConversionAdvisoryBoardDecisionDeferredReasonState.AdvisoryBoardDecisionId))]
    public HashSet<ConversionAdvisoryBoardDecisionDeferredReasonState>? DeferredReasons { get; init; }
    public string? DeferredOtherReason { get; init; }
    public DateTime AdvisoryBoardDecisionDate { get; init; }
    public DecisionMadeBy DecisionMadeBy { get; init; }

    public static ConversionAdvisoryBoardDecisionState MapFromDomain(IConversionAdvisoryBoardDecision decision)
    {
        return new()
        {
            Id = decision.Id,
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
            DecisionMadeBy = decision.AdvisoryBoardDecisionDetails.DecisionMadeBy,
            CreatedOn = decision.CreatedOn,
            LastModifiedOn = decision.LastModifiedOn
        };
    }

    public IConversionAdvisoryBoardDecision MapToDomain()
    {
        var details = new AdvisoryBoardDecisionDetails(
            Id,
            ConversionProjectId,
            Decision,
            ApprovedConditionsSet,
            ApprovedConditionsDetails,
            DeclinedReasons!.Select(reason => reason.Reason).ToList(),
            DeclinedOtherReason,
            DeferredReasons!.Select(reason => reason.Reason).ToList(),
            DeferredOtherReason,
            AdvisoryBoardDecisionDate,
            DecisionMadeBy
        );
        
        return new ConversionAdvisoryBoardDecision(Id, details, CreatedOn, LastModifiedOn);
    }
}
