using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision
{
	public class SignificantChangeDecisionCommand : IRequest<CreateResult>
	{
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

	public enum Decision
	{
		Approved = 0,
		Declined = 1,
		Deferred = 2,
		Withdrawn = 3
	}
}
