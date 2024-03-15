using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IConversionProjectQueryService
	{
		Task<PagedDataResponse<ConversionProjectServiceModel>?> GetProjects(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions, IEnumerable<string>? applicationReferences);

		Task<PagedDataResponse<ConversionProjectServiceModel>?> GetProjectsV2(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates);

		Task<PagedDataResponse<FormAMatProjectServiceModel>?> GetFormAMatProjects(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, CancellationToken cancellationToken, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates);

		Task<ConversionProjectServiceModel?> GetConversionProject(int id);
		Task<FormAMatProjectServiceModel> GetFormAMatProjectById(int id, CancellationToken cancellationToken);
		Task<ProjectFilterParameters> GetFilterParameters();
		Task<IEnumerable<FormAMatProjectServiceModel>> SearchFormAMatProjectsByTermAsync(string searchTerm, CancellationToken cancellationToken);

	}
}
