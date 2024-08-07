using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IProjectGroupQueryService
	{
		Task<ProjectGroupResponseModel> GetProjectGroupByIdAsync(int id, CancellationToken cancellationToken);
		Task<PagedDataResponse<ProjectGroupResponseModel>?> GetProjectGroupsAsync(IEnumerable<string>? states, string? title,
			IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions,
			IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count, CancellationToken cancellationToken);
	}
}
