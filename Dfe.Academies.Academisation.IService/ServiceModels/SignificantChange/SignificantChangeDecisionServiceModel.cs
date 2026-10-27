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
	public string? DeclinedReason { get; init; }
	public string? WithdrawnReason { get; init; }
	public DateTime DecisionDate { get; set; }
	public SignificantChangeDecisionMadeBy DecisionMadeBy { get; set; }
	public string? DecisionMakerName { get; set; }
}
