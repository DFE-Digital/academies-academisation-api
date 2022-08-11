using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

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
			AdvisoryBoardDecisionDate = decision.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate,
			DecisionMadeBy = decision.AdvisoryBoardDecisionDetails.DecisionMadeBy
		};
	}
}
