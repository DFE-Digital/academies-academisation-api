﻿namespace Dfe.Academies.Academisation.Domain.Core;

public record AdvisoryBoardDecisionDetails(
	int ConversionProjectId,
	AdvisoryBoardDecisions Decision,
	bool? ApprovedConditionsSet,
	string? ApprovedConditionsDetails,
	List<AdvisoryBoardDeclinedReasons>? DeclinedReasons,
	string? DeclinedOtherReason,
	List<AdvisoryBoardDeferredReasons>? DeferredReasons,
	string? DeferredOtherReason,
	DateTime AdvisoryBoardDecisionDate,
	DecisionMadeBy DecisionMadeBy
);
