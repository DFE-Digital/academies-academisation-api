using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.Service.Mappers;

internal static class AdvisoryBoardDecisionCreateRequestModelMapper
{
    internal static AdvisoryBoardDecisionDetails AsDomain(this AdvisoryBoardDecisionCreateRequestModel model)
    {
        return new(
            default,
            model.ConversionProjectId,
            model.Decision,
            model.ApprovedConditionsSet,
            model.ApprovedConditionsDetails,
            model.DeclinedReasons,
            model.DeclinedOtherReason,
            model.DeferredReasons,
            model.DeferredOtherReason,
            model.AdvisoryBoardDecisionDate,
            model.DecisionMadeBy
        );
    }
}
