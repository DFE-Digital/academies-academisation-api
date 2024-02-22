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

	[ForeignKey(nameof(AdvisoryBoardDecisionDeclinedReasonState.AdvisoryBoardDecisionId))]
	public HashSet<AdvisoryBoardDecisionDeclinedReasonState>? DeclinedReasons { get; init; }

	[ForeignKey(nameof(AdvisoryBoardDecisionDeferredReasonState.AdvisoryBoardDecisionId))]
	public HashSet<AdvisoryBoardDecisionDeferredReasonState>? DeferredReasons { get; init; }

	[ForeignKey(nameof(AdvisoryBoardDecisionWithdrawnReasonState.AdvisoryBoardDecisionId))]
	public HashSet<AdvisoryBoardDecisionWithdrawnReasonState>? WithdrawnReasons { get; init; }


	public DateTime AdvisoryBoardDecisionDate { get; init; }
	public DateTime AcademyOrderDate { get; init; }
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
				.Select(reason => new AdvisoryBoardDecisionDeclinedReasonState
				{
					Reason = reason.Reason,
					Details = reason.Details
				})
				.ToHashSet(),
			DeferredReasons = decision.AdvisoryBoardDecisionDetails.DeferredReasons?
				.Select(reason => new AdvisoryBoardDecisionDeferredReasonState
				{
					Reason = reason.Reason,
					Details = reason.Details
				})
				.ToHashSet(),
			WithdrawnReasons = decision.AdvisoryBoardDecisionDetails.WithdrawnReasons?
				.Select(reason => new AdvisoryBoardDecisionWithdrawnReasonState
				{
					Reason = reason.Reason,
					Details = reason.Details
				})
				.ToHashSet(),
			AdvisoryBoardDecisionDate = decision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate,
			AcademyOrderDate = decision.AdvisoryBoardDecisionDetails.AcademyOrderDate,
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
			WithdrawnReasons?
				.Select(reason => new AdvisoryBoardWithdrawnReasonDetails(reason.Reason, reason.Details))
				.ToList(),
			AdvisoryBoardDecisionDate,
			AcademyOrderDate,
			DecisionMadeBy
		);

		return new ConversionAdvisoryBoardDecision(Id, details, CreatedOn, LastModifiedOn);
	}
}
