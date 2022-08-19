using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService.Commands;

public interface IAdvisoryBoardDecisionCreateCommand
{
	Task<CreateResult<ConversionAdvisoryBoardDecisionServiceModel>> Execute(AdvisoryBoardDecisionCreateRequestModel requestModel);
}
