using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IProjectGroupQueryService
	{
		Task<PagedDataResponse<ProjectGroupResponseModel>?> GetProjectGroupsAsync(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, CancellationToken cancellationToken, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates);
	}
}
