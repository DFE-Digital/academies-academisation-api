using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

namespace Dfe.Academies.Academisation.IService.Commands.Application.School
{
	public interface IDeleteLeaseCommandHandler
	{
		Task<CommandResult> Handle(DeleteLeaseCommand leaseCommand);
	}
}
