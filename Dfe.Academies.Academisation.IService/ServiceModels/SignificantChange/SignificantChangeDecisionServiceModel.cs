using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

public class SignificantChangeDecisionServiceModel
{
	public int AdvisoryBoardDecisionId { get; init; }
	public int? SignificantChangeProjectId { get; init; }
	public Decision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; }
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; }
	public List<AdvisoryBoardWithdrawnReasonDetails>? WithdrawnReasons { get; init; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
	public string? DecisionMakerName { get; set; }
}
