using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService;

public interface IApplicationCreateCommand
{
	Task<CreateResult<ApplicationServiceModel>> Execute(ApplicationCreateRequestModel conversionApplicationRequestModel);
}
