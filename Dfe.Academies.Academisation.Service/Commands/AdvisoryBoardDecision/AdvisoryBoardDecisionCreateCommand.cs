using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;

public class AdvisoryBoardDecisionCreateCommand : IRequest<CreateResult>
{
	public int? ConversionProjectId { get; init; }
	public int? TransferProjectId { get; init; }
	public Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }
	public List<AdvisoryBoardDeclinedReasonDetails>? DeclinedReasons { get; init; }
	public List<AdvisoryBoardDeferredReasonDetails>? DeferredReasons { get; init; }
	public List<AdvisoryBoardWithdrawnReasonDetails>? WithdrawnReasons { get; init; }
	public DateTime AdvisoryBoardDecisionDate { get; set; }
	public DateTime? AcademyOrderDate { get; set; }
	public DecisionMadeBy DecisionMadeBy { get; set; }
	public string DecisionMakersName { get; set; }
}
