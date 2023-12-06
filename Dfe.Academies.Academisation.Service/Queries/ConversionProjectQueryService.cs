using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries;

public class ConversionProjectQueryService : IConversionProjectQueryService
{
	private readonly IConversionProjectRepository _conversionProjectRepository;

	public ConversionProjectQueryService(IConversionProjectRepository conversionProjectRepository)
	{
		_conversionProjectRepository = conversionProjectRepository;
	}

	public async Task<LegacyProjectServiceModel?> GetConversionProject(int id)
	{
		var project = await this._conversionProjectRepository.GetConversionProject(id).ConfigureAwait(false);

		return project?.MapToServiceModel();

	}

	public async Task<LegacyApiResponse<LegacyProjectServiceModel>?> GetProjects(
		IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions, IEnumerable<string>? applicationReferences)
	{

		var (projects, totalCount) = await _conversionProjectRepository.SearchProjects(
												states, title, deliveryOfficers, page, count, urn, regions, applicationReferences);

		var pageResponse = PagingResponseFactory.Create("legacy/projects", page, count, totalCount,
			new Dictionary<string, object?> {
				{"states", states},
				{"urn", urn}
			});

		var data = projects.Select(p => p.MapToServiceModel());

		return new LegacyApiResponse<LegacyProjectServiceModel>(data,
			pageResponse);
	}

	public async Task<ProjectFilterParameters> GetFilterParameters()
	{
		return await _conversionProjectRepository.GetFilterParameters();
	}
}
