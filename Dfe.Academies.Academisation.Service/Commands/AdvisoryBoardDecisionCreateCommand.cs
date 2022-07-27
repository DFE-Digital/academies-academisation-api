using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Mappers;

namespace Dfe.Academies.Academisation.Service.Commands;

public class AdvisoryBoardDecisionCreateCommand : IAdvisoryBoardDecisionCreateCommand
{
    private readonly IAdvisoryBoardDecisionCreateDataCommand _createDataCommand;
    private readonly IConversionAdvisoryBoardDecisionFactory _factory;

    public AdvisoryBoardDecisionCreateCommand(IConversionAdvisoryBoardDecisionFactory factory,
        IAdvisoryBoardDecisionCreateDataCommand createDataCommand)
    {
        _createDataCommand = createDataCommand;
        _factory = factory;
    }

    public async Task<CreateResult<ConversionAdvisoryBoardDecisionServiceModel>> Execute(
        AdvisoryBoardDecisionCreateRequestModel requestModel)
    {
        var result = _factory.Create(requestModel.AsDomain());

        return result switch
        {
            CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult =>
                await ExecuteDataCommand(successResult),
            CreateValidationErrorResult<IConversionAdvisoryBoardDecision> errorResult =>
                errorResult.MapToPayloadType<ConversionAdvisoryBoardDecisionServiceModel>(), 
            _ => throw new NotImplementedException($"Other CreateResult types not expected ({result.GetType()}")
        };
    }

    private async Task<CreateResult<ConversionAdvisoryBoardDecisionServiceModel>> ExecuteDataCommand(
        CreateSuccessResult<IConversionAdvisoryBoardDecision> successResult)
    {
        await _createDataCommand.Execute(successResult.Payload);
        
        return successResult.MapToPayloadType(ConversionAdvisoryBoardDecisionServiceModelMapper.MapFromDomain);
    }
}
