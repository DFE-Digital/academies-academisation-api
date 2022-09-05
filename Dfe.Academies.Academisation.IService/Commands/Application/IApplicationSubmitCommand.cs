using Dfe.Academies.Academisation.Core;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public interface IApplicationSubmitCommand
	{
		Task<CommandResult> Execute(int applicationId);
	}
}
