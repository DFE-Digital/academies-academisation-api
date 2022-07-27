using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.IService.Commands;

public interface IAdvisoryBoardDecisionCreateCommand
{
    Task Execute(AdvisoryBoardDecisionCreateRequestModel requestModel);
}
