using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

public static class AdvisoryBoardDecisionDetailsMapper
{
	public static AdvisoryBoardDecisionDetails ToDomain(this ConversionAdvisoryBoardDecisionServiceModel serviceModel)
	{
		return new(
			serviceModel.ConversionProjectId,
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
