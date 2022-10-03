using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.IService.Commands.Application;

public interface ISetJoinTrustDetailsCommandHandler
{
	Task<CommandResult> Handle(int applicationId, SetJoinTrustDetailsCommand command);
}
