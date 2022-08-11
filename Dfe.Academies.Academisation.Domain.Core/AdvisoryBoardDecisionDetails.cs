namespace Dfe.Academies.Academisation.Domain.Core;

public record AdvisoryBoardDecisionDetails(
	int ConversionProjectId,
	AdvisoryBoardDecision Decision,
	bool? ApprovedConditionsSet,
	string? ApprovedConditionsDetails,
	List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons,
	List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons,
	DateTime AdvisoryBoardDecisionDate,
	DecisionMadeBy DecisionMadeBy
);
