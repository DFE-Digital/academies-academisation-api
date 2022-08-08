namespace Dfe.Academies.Academisation.Domain.Core;

public record AdvisoryBoardDecisionDetails(
	int AdvisoryBoardDecisionId,
	int ConversionProjectId,
	AdvisoryBoardDecision Decision,
	bool? ApprovedConditionsSet,
	string? ApprovedConditionsDetails,
	List<AdvisoryBoardDeclinedReason>? DeclinedReasons,
	string? DeclinedOtherReason,
	List<AdvisoryBoardDeferredReason>? DeferredReasons,
	string? DeferredOtherReason,
	DateTime AdvisoryBoardDecisionDate,
	DecisionMadeBy DecisionMadeBy
);
