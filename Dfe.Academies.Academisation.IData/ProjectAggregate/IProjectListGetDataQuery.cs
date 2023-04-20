using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectListGetDataQuery
	{
		Task<(IEnumerable<IProject>, int)> SearchProjects(
			IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions = default, IEnumerable<string>? applicationReferences = default);
	}
}
