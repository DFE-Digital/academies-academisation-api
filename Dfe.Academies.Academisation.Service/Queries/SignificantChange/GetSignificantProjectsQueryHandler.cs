using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Factories;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public class GetSignificantProjectsQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository) : IRequestHandler<GetSignificantProjectsQuery, PagedDataResponse<SignificantChangeProjectSearchResponse>>
	{
		private readonly ISignificantChangeProjectRepository _significantChangeProjectRepository = significantChangeProjectRepository;

		public async Task<PagedDataResponse<SignificantChangeProjectSearchResponse>> Handle(GetSignificantProjectsQuery query, CancellationToken cancellationToken)
		{
			var (projects, totalCount) = await _significantChangeProjectRepository.SearchSignificantProjects(query.Page, query.Count, cancellationToken);

			var routeValues = new Dictionary<string, object?>();
			var pageResponse = PagingResponseFactory.Create("significant-change/significant-change-projects", query.Page, query.Count, totalCount, routeValues);

			var data = projects.Select(p => p.MapToServiceModel());

			return new PagedDataResponse<SignificantChangeProjectSearchResponse>(data,
				pageResponse);
		}

	}
}
