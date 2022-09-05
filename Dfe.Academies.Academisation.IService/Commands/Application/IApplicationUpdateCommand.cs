using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Commands.Application;

public interface IApplicationUpdateCommand
{
	Task<CommandResult> Execute(int applicationId, ApplicationServiceModel applicationServiceModel);
}
