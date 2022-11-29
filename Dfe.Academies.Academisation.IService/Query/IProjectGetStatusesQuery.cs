using Dfe.Academies.Academisation.Domain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IProjectGetStatusesQuery
	{
		Task<ProjectFilterParameters> Execute();
	}
}
