using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectListGetDataQuery
	{
		Task<(IEnumerable<IProject>, int)> SearchProjects(
			string[]? states, string? title, string[]? deliveryOfficers, int page, int count, int? urn,int[]? regions = default);
	}
}
