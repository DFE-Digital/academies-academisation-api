using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ILegacyProjectListGetQuery
	{
		Task<LegacyApiResponse<LegacyProjectServiceModel>?> GetProjects(
			string? states, string? title, string[]? deliveryOfficers, int page, int count, int? urn);
	}
}
