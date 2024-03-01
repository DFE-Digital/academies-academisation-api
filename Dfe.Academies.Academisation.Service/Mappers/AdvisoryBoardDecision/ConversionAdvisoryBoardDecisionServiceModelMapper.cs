using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

internal static class ConversionAdvisoryBoardDecisionServiceModelMapper
{
	internal static ConversionAdvisoryBoardDecisionServiceModel MapFromDomain(
		this IConversionAdvisoryBoardDecision decision)
	{
		return new()
		{
			AdvisoryBoardDecisionId = decision.Id,
			ConversionProjectId = decision.AdvisoryBoardDecisionDetails.ConversionProjectId,
			Decision = decision.AdvisoryBoardDecisionDetails.Decision,
			ApprovedConditionsSet = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet,
			ApprovedConditionsDetails = decision.AdvisoryBoardDecisionDetails.ApprovedConditionsDetails,
			DeclinedReasons = decision.AdvisoryBoardDecisionDetails.DeclinedReasons,
			DeferredReasons = decision.AdvisoryBoardDecisionDetails.DeferredReasons,
			WithdrawnReasons = decision.AdvisoryBoardDecisionDetails.WithdrawnReasons,
			AdvisoryBoardDecisionDate = decision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate,
			AcademyOrderDate = decision.AdvisoryBoardDecisionDetails.AcademyOrderDate,
			DecisionMadeBy = decision.AdvisoryBoardDecisionDetails.DecisionMadeBy
		};
	}
}
