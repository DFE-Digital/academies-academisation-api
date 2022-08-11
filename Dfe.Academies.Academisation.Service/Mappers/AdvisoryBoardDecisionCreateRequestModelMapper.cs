using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class AdvisoryBoardDecisionCreateRequestModelMapper
{
    internal static AdvisoryBoardDecisionDetails AsDomain(this AdvisoryBoardDecisionCreateRequestModel model)
    {
        return new(
            model.ConversionProjectId,
            model.Decision,
            model.ApprovedConditionsSet,
            model.ApprovedConditionsDetails,
            model.DeclinedReasons,
            model.DeferredReasons,
            model.AdvisoryBoardDecisionDate,
            model.DecisionMadeBy
        );
    }
}
