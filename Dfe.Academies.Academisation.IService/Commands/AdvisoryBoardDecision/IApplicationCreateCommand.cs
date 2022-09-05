using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Commands.AdvisoryBoardDecision;

public interface IApplicationCreateCommand
{
	Task<CreateResult<ApplicationServiceModel>> Execute(ApplicationCreateRequestModel applicationRequestModel);
}
