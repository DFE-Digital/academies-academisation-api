using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IConversionProjectQueryService
	{
		Task<LegacyApiResponse<LegacyProjectServiceModel>?> GetProjects(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions, IEnumerable<string>? applicationReferences);

		Task<LegacyProjectServiceModel?> GetConversionProject(int id);

		Task<ProjectFilterParameters> GetFilterParameters();

	}
}
