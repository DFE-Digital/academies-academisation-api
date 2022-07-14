using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;

public record AdvisoryBoardDecisionDetails(
	AdvisoryBoardDecisions Decision,
	bool? ApprovedConditionsSet,
	string? ApprovedConditionsDetails,
	List<AdvisoryBoardDeclinedReasons>? DeclinedReasons,
	string? DeclinedOtherReason,
	List<AdvisoryBoardDeferredReasons>? DeferredReasons,
	string? DeferredOtherReason,
	DateTime AdvisoryBoardDecisionDate
) : IAdvisoryBoardDecisionDetails;