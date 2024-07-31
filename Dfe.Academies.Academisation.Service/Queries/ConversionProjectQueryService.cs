using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
namespace Dfe.Academies.Academisation.Service.Queries;

public class ConversionProjectQueryService : IConversionProjectQueryService
{
	private readonly IConversionProjectRepository _conversionProjectRepository;
	private readonly IFormAMatProjectRepository _formAMatProjectRepository;

	public ConversionProjectQueryService(IConversionProjectRepository conversionProjectRepository, IFormAMatProjectRepository formAMatProjectRepository)
	{
		_conversionProjectRepository = conversionProjectRepository;
		_formAMatProjectRepository = formAMatProjectRepository;
	}

	public async Task<ConversionProjectServiceModel?> GetConversionProject(int id, CancellationToken cancellationToken)
	{
		var project = await this._conversionProjectRepository.GetConversionProject(id, cancellationToken).ConfigureAwait(false);

		return project?.MapToServiceModel();

	}

	public async Task<PagedDataResponse<ConversionProjectServiceModel>?> GetProjects(
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

		return new PagedDataResponse<ConversionProjectServiceModel>(data,
			pageResponse);
	}

	public async Task<ProjectFilterParameters> GetFilterParameters()
	{
		return await _conversionProjectRepository.GetFilterParameters();
	}

	public async Task<PagedDataResponse<ConversionProjectServiceModel>?> GetProjectsV2(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates)
	{
		var (projects, totalCount) = await _conversionProjectRepository.SearchProjectsV2(states, title, deliveryOfficers, regions, localAuthorities, advisoryBoardDates, page, count);

		var pageResponse = PagingResponseFactory.Create("conversion-projects/projects", page, count, totalCount,
			new Dictionary<string, object?> {
				{"states", states},
			});

		var data = projects.Select(p => p.MapToServiceModel());

		return new PagedDataResponse<ConversionProjectServiceModel>(data,
			pageResponse);
	}

	public async Task<PagedDataResponse<FormAMatProjectServiceModel>?> GetFormAMatProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, CancellationToken cancellationToken, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates)
	{
		var (projects, totalCount) = await _conversionProjectRepository.SearchFormAMatProjects(states, title, deliveryOfficers, regions, localAuthorities, advisoryBoardDates);

		var formAMatAggregates = await _formAMatProjectRepository.GetByIds(projects.Select(x => x.FormAMatProjectId).Distinct(), cancellationToken).ConfigureAwait(false);

		// doing paging here as it has to be at a form a mat level
		formAMatAggregates = formAMatAggregates.OrderByDescending(x => x.CreatedOn).Skip((page - 1) * count)
				.Take(count).ToList();

		// need to add back in any projects that were filtered out, otherwise the project list indicates there are less projects in the form a mat than there really is
		projects = projects.Union(await _conversionProjectRepository.GetConversionProjectsByFormAMatIds(formAMatAggregates.Select(x => x.Id).Cast<int?>(), cancellationToken));

		var pageResponse = PagingResponseFactory.Create("conversion-projects/FormAMatProjects", page, count, totalCount,
			new Dictionary<string, object?> {
				{"states", states},
			});

		var data = formAMatAggregates.Select(p => p.MapToFormAMatServiceModel(projects));

		return new PagedDataResponse<FormAMatProjectServiceModel>(data,
			pageResponse);
	}
	public async Task<FormAMatProjectServiceModel> GetFormAMatProjectById(int id, CancellationToken cancellationToken)
	{
		FormAMatProject project = await _formAMatProjectRepository.GetById(id);
		var relatedProjects = await _conversionProjectRepository.GetConversionProjectsByFormAMatId(project.Id, cancellationToken).ConfigureAwait(false);

		return project.MapToFormAMatServiceModel(relatedProjects);
	}
	public async Task<IEnumerable<FormAMatProjectServiceModel>> SearchFormAMatProjectsByTermAsync(string searchTerm, CancellationToken cancellationToken)
	{
		var projects = await _formAMatProjectRepository.SearchProjectsByTermAsync(searchTerm, cancellationToken);

		var serviceModels = projects.Select(project => new FormAMatProjectServiceModel(
			project.Id,
			project.ProposedTrustName,
			project.ApplicationReference,
			new User(project.AssignedUser?.Id ?? Guid.Empty, project.AssignedUser?.FullName ?? string.Empty, project.AssignedUser?.EmailAddress ?? string.Empty),
			project.ReferenceNumber
		)).ToList();

		return serviceModels;
	}

	public async Task<IEnumerable<SchoolImprovementPlanServiceModel>> GetSchoolImprovementPlansByConversionProjectId(int id, CancellationToken cancellationToken)
	{
		var project = await _conversionProjectRepository.GetById(id);

		var serviceModels = project.SchoolImprovementPlans.Select(x => new SchoolImprovementPlanServiceModel(
			x.Id,
			x.ProjectId,
			x.ArrangedBy,
			x.ArrangedByOther,
			x.ProvidedBy,
			x.StartDate,
			x.ExpectedEndDate,
			x.ExpectedEndDateOther,
			x.ConfidenceLevel,
			x.PlanComments,
			x.CreatedOn));

		return serviceModels;
	}

	public async Task<List<ConversionProjectServiceModel>> GetProjectsForGroup(string trustReferenceNumber, CancellationToken cancellationToken)
	{
		// get conversion projects
		var projects = await this._conversionProjectRepository.GetConversionProjectsForNewGroup(trustReferenceNumber, cancellationToken).ConfigureAwait(false) ?? new List<Project>();

		// get transfer projects will go here

		return projects.Select(x => x.MapToServiceModel()).ToList();
	}
}
