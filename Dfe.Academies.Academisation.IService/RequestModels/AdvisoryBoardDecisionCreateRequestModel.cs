﻿using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.RequestModels
{
    public interface AdvisoryBoardDecisionCreateRequestModel
	{
		AdvisoryBoardDecisions Decision { get; }
		bool? ApprovedConditionsSet { get; }
		string? ApprovedConditionsDetails { get; }
		List<AdvisoryBoardDeclinedReasons>? DeclinedReasons { get; }
		string? DeclinedOtherReason { get; }
		List<AdvisoryBoardDeferredReasons>? DeferredReasons { get; }
		string? DeferredOtherReason { get; }
		DateTime AdvisoryBoardDecisionDate { get; }
		DecisionMadeBy DecisionMadeBy { get; }
	}
}
