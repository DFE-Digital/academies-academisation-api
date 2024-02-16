namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public record AdvisoryBoardDecisionDetails(
	int? ConversionProjectId,
	int? TransferProjectId,
	AdvisoryBoardDecision Decision,
	bool? ApprovedConditionsSet,
	string? ApprovedConditionsDetails,
	List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons,
	List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons,
	DateTime AdvisoryBoardDecisionDate,
	DecisionMadeBy DecisionMadeBy
);
