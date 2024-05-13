using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionUpdateCommand : IRequest<CommandResult>
{
	public int AdvisoryBoardDecisionId { get; init; }
	public int? ConversionProjectId { get; init; }
	public int? TransferProjectId { get; init; }
	public Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; } = new();
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; } = new();
	public List<AdvisoryBoardWithdrawnReasonDetails>? WithdrawnReasons { get; init; } = new();
	public DateTime AdvisoryBoardDecisionDate { get; init; }
	public DateTime? AcademyOrderDate { get; init; }
	public DecisionMadeBy DecisionMadeBy { get; init; }

	public string DecisionMakerName { get; set; }
}
