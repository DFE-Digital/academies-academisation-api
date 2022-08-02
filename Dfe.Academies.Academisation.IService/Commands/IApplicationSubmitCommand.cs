using Dfe.Academies.Academisation.Core;

namespace Dfe.Academies.Academisation.IService.Commands
{
	public interface IApplicationSubmitCommand
	{
		Task<CommandResult> Execute(int applicationId);
	}
}