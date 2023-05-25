using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.IService.RequestModels;

/// <summary>
/// Represents a request model sent to the API.
/// Note that this type is replicated in the Web API Contracts project
/// </summary>
public class AdvisoryBoardDecisionCreateRequestModel
{
	public int ConversionProjectId { get; init; }
	public AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; }
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
}
