using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;

[Table(name: "AdvisoryBoardDecision")]
public class AdvisoryBoardDecisionState : BaseEntity
{
	public int? ConversionProjectId { get; init; }
	public int? TransferProjectId { get; init; }
	public AdvisoryBoardDecision Decision { get; init; }
	public bool? ApprovedConditionsSet { get; init; }
	public string? ApprovedConditionsDetails { get; init; }

	[ForeignKey(nameof(ConversionAdvisoryBoardDecisionDeclinedReasonState.AdvisoryBoardDecisionId))]
	public HashSet<ConversionAdvisoryBoardDecisionDeclinedReasonState>? DeclinedReasons { get; init; }

	[ForeignKey(nameof(ConversionAdvisoryBoardDecisionDeferredReasonState.AdvisoryBoardDecisionId))]
	public HashSet<ConversionAdvisoryBoardDecisionDeferredReasonState>? DeferredReasons { get; init; }



	public DateTime AdvisoryBoardDecisionDate { get; init; }
	public DecisionMadeBy DecisionMadeBy { get; init; }

	public static AdvisoryBoardDecisionState MapFromDomain(IConversionAdvisoryBoardDecision decision)
	{
		return new()
		{
			Id = decision.Id,
			ConversionProjectId = decision.AdvisoryBoardDecisionDetails.ConversionProjectId,
			TransferProjectId = decision.AdvisoryBoardDecisionDetails.TransferProjectId,
			Decision = decision.AdvisoryBoardDecisionDetails.Decision,
			ApprovedConditionsSet = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet,
			ApprovedConditionsDetails = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails,
			DeclinedReasons = decision.AdvisoryBoardDecisionDetails.DeclinedReasons?
				.Select(reason => new ConversionAdvisoryBoardDecisionDeclinedReasonState
				{
					Reason = reason.Reason,
					Details = reason.Details
				})
				.ToHashSet(),
			DeferredReasons = decision.AdvisoryBoardDecisionDetails.DeferredReasons?
				.Select(reason => new ConversionAdvisoryBoardDecisionDeferredReasonState
				{
					Reason = reason.Reason,
					Details = reason.Details
				})
				.ToHashSet(),
			AdvisoryBoardDecisionDate = decision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate,
			DecisionMadeBy = decision.AdvisoryBoardDecisionDetails.DecisionMadeBy,
			CreatedOn = decision.CreatedOn,
			LastModifiedOn = decision.LastModifiedOn
		};
	}

	public IConversionAdvisoryBoardDecision MapToDomain()
	{
		var details = new AdvisoryBoardDecisionDetails(
			ConversionProjectId,
			TransferProjectId,
			Decision,
			ApprovedConditionsSet,
			ApprovedConditionsDetails,
			DeclinedReasons?
				.Select(reason => new AdvisoryBoardDeclinedReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			DeferredReasons?
				.Select(reason => new AdvisoryBoardDeferredReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			AdvisoryBoardDecisionDate,
			DecisionMadeBy
		);

		return new ConversionAdvisoryBoardDecision(Id, details, CreatedOn, LastModifiedOn);
	}
}
