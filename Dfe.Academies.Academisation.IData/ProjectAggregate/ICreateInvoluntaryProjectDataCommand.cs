
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface ICreateInvoluntaryProjectDataCommand
	{
		Task<CommandResult> Execute(InvoluntaryProject project);
	}
}
