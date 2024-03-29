﻿using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class AdvisoryBoardDecisionCreateRequestModel
{
	public int? ConversionProjectId { get; init; }
	public int? TransferProjectId { get; init; }
	public AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; }
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; }
	public List<AdvisoryBoardWithdrawnReasonDetails>? WithdrawnReasons { get; init; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DateTime? AcademyOrderDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
}
