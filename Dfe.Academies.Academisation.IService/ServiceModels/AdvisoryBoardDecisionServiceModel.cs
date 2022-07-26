using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.ServiceModels;

public class AdvisoryBoardDecisionServiceModel
{
	public int ConversionProjectId { get; set; }
	public AdvisoryBoardDecision Decision { get; set; }
	public bool? ApprovedConditionsSet { get; set; }
	public string? ApprovedConditionsDetails { get; set; }
	public List<AdvisoryBoardDeclinedReason>? DeclinedReasons { get; set; }
	public string? DeclinedOtherReason { get; set; }
	public List<AdvisoryBoardDeferredReason>? DeferredReasons { get; set; }
	public string? DeferredOtherReason { get; set; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
}
