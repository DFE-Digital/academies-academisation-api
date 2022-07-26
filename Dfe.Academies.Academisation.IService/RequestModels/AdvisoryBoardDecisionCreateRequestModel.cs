﻿using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class AdvisoryBoardDecisionCreateRequestModel
{
	public int ConversionProjectId { get; set; }
	public AdvisoryBoardDecisions Decision { get; set; }
	public bool? ApprovedConditionsSet { get; set; }
	public string? ApprovedConditionsDetails { get; set; }
	public List<AdvisoryBoardDeclinedReasons>? DeclinedReasons { get; set; }
	public string? DeclinedOtherReason { get; set; }
	public List<AdvisoryBoardDeferredReasons>? DeferredReasons { get; set; }
	public string? DeferredOtherReason { get; set; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
}
