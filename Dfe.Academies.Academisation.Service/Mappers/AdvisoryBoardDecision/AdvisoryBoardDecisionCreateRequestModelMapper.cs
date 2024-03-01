using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.Service.Mappers.AdvisoryBoardDecision;

internal static class AdvisoryBoardDecisionCreateRequestModelMapper
{
	internal static AdvisoryBoardDecisionDetails AsDomain(this AdvisoryBoardDecisionCreateRequestModel model)
	{
		return new(
			model.ConversionProjectId,
			model.TransferProjectId,
			model.Decision,
			model.ApprovedConditionsSet,
			model.ApprovedConditionsDetails,
			model.AdvisoryBoardDecisionDate,
			model.DecisionMadeBy
		);
	}
}
