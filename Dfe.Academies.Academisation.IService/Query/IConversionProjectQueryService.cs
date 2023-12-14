using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IConversionProjectQueryService
	{
		Task<LegacyApiResponse<ConversionProjectServiceModel>?> GetProjects(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions, IEnumerable<string>? applicationReferences);

		Task<LegacyApiResponse<ConversionProjectServiceModel>?> GetProjects(
IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, IEnumerable<string>? regions,
IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates);

		Task<ConversionProjectServiceModel?> GetConversionProject(int id);

		Task<ProjectFilterParameters> GetFilterParameters();

	}
}
