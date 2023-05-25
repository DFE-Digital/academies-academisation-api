namespace Dfe.Academies.Academisation.WebApi.Contracts.RequestModels;
using Dfe.Academies.Academisation.WebApi.Contracts.FromDomain;

public class AdvisoryBoardDecisionCreateRequestModel
{
	public int ConversionProjectId { get; set; }
	public AdvisoryBoardDecision Decision { get; set; }
	public bool? ApprovedConditionsSet { get; set; }
	public string? ApprovedConditionsDetails { get; set; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; set; }
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; set; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
}
