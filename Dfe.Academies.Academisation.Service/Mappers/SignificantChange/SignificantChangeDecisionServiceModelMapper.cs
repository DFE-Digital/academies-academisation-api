using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

namespace Dfe.Academies.Academisation.Service.Mappers.SignificantChange;

internal static class SignificantChangeDecisionServiceModelMapper
{
	internal static SignificantChangeDecisionServiceModel MapFromDomain(
		this IConversionAdvisoryBoardDecision decision)
	{
		return new()
		{
			AdvisoryBoardDecisionId = decision.Id,
			SignificantChangeProjectId = decision.AdvisoryBoardDecisionDetails.SignificantChangeProjectId,
			Decision = decision.AdvisoryBoardDecisionDetails.Decision.MapToSignificantChangeDecision(),
			ApprovedConditionsSet = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet,
			ApprovedConditionsDetails = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails,
			DeclinedReasons = decision.DeclinedReasons.ToList(),
			DeferredReasons = decision.DeferredReasons.ToList(),
			WithdrawnReasons = decision.WithdrawnReasons.ToList(),
			DecisionDate = decision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate,
			DecisionMadeBy = decision.AdvisoryBoardDecisionDetails.DecisionMadeBy.MapToSignificantChangeDecisionMadeBy(),
			DecisionMakerName = decision.AdvisoryBoardDecisionDetails.DecisionMakerName
		};
	}

	internal static Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision MapToAdvisoryBoardDecision(
		this Decision decision) =>
		decision switch
		{
			Decision.Approved => Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Approved,
			Decision.Declined => Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Declined,
			Decision.Deferred => Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Deferred,
			Decision.Withdrawn => Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Withdrawn,
			_ => throw new ArgumentOutOfRangeException(nameof(decision), decision, null)
		};

	internal static Decision MapToSignificantChangeDecision(this
		Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision decision) =>
		decision switch
		{
			Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Approved => Decision.Approved,
			Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Declined => Decision.Declined,
			Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Deferred => Decision.Deferred,
			Domain.Core.ConversionAdvisoryBoardDecisionAggregate.AdvisoryBoardDecision.Withdrawn => Decision.Withdrawn,
			_ => throw new ArgumentOutOfRangeException(nameof(decision), decision, null)
		};

	internal static DecisionMadeBy MapToAdvisoryBoardDecisionMadeBy(this SignificantChangeDecisionMadeBy decisionMadeBy) =>
		decisionMadeBy switch
		{
			SignificantChangeDecisionMadeBy.RegionalDirector => DecisionMadeBy.RegionalDirectorForRegion,
			SignificantChangeDecisionMadeBy.Minister => DecisionMadeBy.Minister,
			SignificantChangeDecisionMadeBy.DirectorGeneral => DecisionMadeBy.DirectorGeneral,
			SignificantChangeDecisionMadeBy.DeputyDirector => DecisionMadeBy.DeputyDirector,
			SignificantChangeDecisionMadeBy.Grade6 => DecisionMadeBy.Grade6,
			_ => throw new ArgumentOutOfRangeException(nameof(decisionMadeBy), decisionMadeBy, null)
		};

	internal static SignificantChangeDecisionMadeBy MapToSignificantChangeDecisionMadeBy(this DecisionMadeBy decisionMadeBy) =>
		decisionMadeBy switch
		{
			DecisionMadeBy.RegionalDirectorForRegion => SignificantChangeDecisionMadeBy.RegionalDirector,
			DecisionMadeBy.Minister => SignificantChangeDecisionMadeBy.Minister,
			DecisionMadeBy.DirectorGeneral => SignificantChangeDecisionMadeBy.DirectorGeneral,
			DecisionMadeBy.DeputyDirector => SignificantChangeDecisionMadeBy.DeputyDirector,
			DecisionMadeBy.Grade6 => SignificantChangeDecisionMadeBy.Grade6,
			_ => throw new ArgumentOutOfRangeException(nameof(decisionMadeBy), decisionMadeBy, null)
		};
}
