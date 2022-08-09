using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class AdvisoryBoardDecisionCreateRequestModel
{
	public int ConversionProjectId { get; init; }
	public AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReason>? DeclinedReasons { get; init; }
	public string? DeclinedOtherReason { get; init; }
	public List<AdvisoryBoardDeferredReason>? DeferredReasons { get; init; }
	public string? DeferredOtherReason { get; set; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
}
