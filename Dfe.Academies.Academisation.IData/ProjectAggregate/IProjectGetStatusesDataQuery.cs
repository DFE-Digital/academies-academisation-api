using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectStatusesDataQuery
	{
		Task<ProjectFilterParameters> Execute();
	}
}
