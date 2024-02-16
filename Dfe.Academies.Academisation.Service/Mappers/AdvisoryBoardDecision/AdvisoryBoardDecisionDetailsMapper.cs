using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;

namespace Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

public static class AdvisoryBoardDecisionDetailsMapper
{
	public static AdvisoryBoardDecisionDetails ToDomain(this ConversionAdvisoryBoardDecisionServiceModel serviceModel)
	{
		return new(
			serviceModel.ConversionProjectId,
			serviceModel.TransferProjectId,
			serviceModel.Decision,
			serviceModel.ApprovedConditionsSet,
			serviceModel.ApprovedConditionsDetails,
			serviceModel.DeclinedReasons,
			serviceModel.DeferredReasons,
			serviceModel.AdvisoryBoardDecisionDate,
			serviceModel.DecisionMadeBy
		);
	}
}
