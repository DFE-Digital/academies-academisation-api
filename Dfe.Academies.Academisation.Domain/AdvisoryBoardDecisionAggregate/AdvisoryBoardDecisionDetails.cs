using Dfe.Academies.Academisation.IDomain.AdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.AdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDecisionDetails(

    AdvisoryBoardDecisions Decision,
    bool? ApprovedConditionsSet,
    string? ApprovedConditionsDetails,
    List<AdvisoryBoardDeclinedReasons>? DeclinedReasons,
    string? DeclinedOtherReason,
    List<AdvisoryBoardDeferredReasons>? DeferredReasons,
    string DeferredOtherReason,
    DateTime AdvisoryBoardDecisionDate
) : IAdvisoryBoardDecisionDetails;