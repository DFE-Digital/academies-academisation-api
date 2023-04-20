using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.RequestModels;

namespace Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;

public interface IAdvisoryBoardDecisionCreateCommand
{
	Task<CreateResult> Execute(AdvisoryBoardDecisionCreateRequestModel requestModel);
}
