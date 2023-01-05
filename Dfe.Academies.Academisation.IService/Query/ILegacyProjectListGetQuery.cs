using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ILegacyProjectListGetQuery
	{
		Task<LegacyApiResponse<LegacyProjectServiceModel>?> GetProjects(
			IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions);
	}
}
