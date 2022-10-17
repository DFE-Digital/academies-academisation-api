using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;

namespace Dfe.Academies.Academisation.IService.Commands.Application.School
{
	public interface ICreateLoanCommandHandler
	{
		Task<CommandResult> Handle(CreateLoanCommand loanCommand);
	}
}
