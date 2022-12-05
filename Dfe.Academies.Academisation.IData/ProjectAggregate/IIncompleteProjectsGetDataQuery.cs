using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IIncompleteProjectsGetDataQuery
	{
		Task<IEnumerable<IProject>?> GetIncompleteProjects();
	}
}
