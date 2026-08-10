using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;

public class SignificantChangeUpdateDecisionCommand : IRequest<CommandResult>
{
	public int AdvisoryBoardDecisionId { get; init; }
	public int? SignificantChangeProjectId { get; init; }
	public Decision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; } = [];
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; } = [];
	public List<AdvisoryBoardWithdrawnReasonDetails>? WithdrawnReasons { get; init; } = [];
	public DateTime DecisionDate { get; init; }
	public DecisionMadeBy DecisionMadeBy { get; init; }
	public string? DecisionMakerName { get; set; }
}
