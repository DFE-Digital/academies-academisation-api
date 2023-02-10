using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface ICreateInvoluntaryProjectCommand
	{
		Task<CommandResult> Execute(InvoluntaryProject project);
	}
}
