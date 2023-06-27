using Dfe.Academies.Academisation.Core;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface ITestProjectDeleteDataQuery
	{
		Task<CommandResult> Execute(int projectId);
	}
}
