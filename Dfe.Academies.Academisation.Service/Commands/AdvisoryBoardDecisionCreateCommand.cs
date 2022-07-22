using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Commands;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.Service.Mappers;

namespace Dfe.Academies.Academisation.Service.Commands;

public class AdvisoryBoardDecisionCreateCommand : IAdvisoryBoardDecisionCreateCommand
{
    private readonly IAdvisoryBoardDecisionCreateDataCommand _createDataCommand;
    private readonly IConversionAdvisoryBoardDecisionFactory _factory;

    public AdvisoryBoardDecisionCreateCommand(IConversionAdvisoryBoardDecisionFactory factory, IAdvisoryBoardDecisionCreateDataCommand createDataCommand)
    {
        _createDataCommand = createDataCommand;
        _factory = factory;
    }

    public async Task Execute(AdvisoryBoardDecisionCreateRequestModel requestModel)
    {
        var decision = await _factory.Create(requestModel.AsDomain());            
            
        await _createDataCommand.Execute(decision);
    }
}
