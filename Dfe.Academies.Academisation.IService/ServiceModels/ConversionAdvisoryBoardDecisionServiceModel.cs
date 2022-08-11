using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.ServiceModels;

public class ConversionAdvisoryBoardDecisionServiceModel
{
	public int AdvisoryBoardDecisionId { get; init; }
	public int ConversionProjectId { get; init; }
	public AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; }
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; }
	public DateTime AdvisoryBoardDecisionDate { get; init; }
	public DecisionMadeBy DecisionMadeBy { get; init; }
}
