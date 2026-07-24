using AutoMapper;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Factories;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public class GetSignificantProjectsQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository, IMapper mapper) : IRequestHandler<GetSignificantProjectsQuery, PagedDataResponse<SignificantChangeProjectSearchResponse>>
	{
		private readonly ISignificantChangeProjectRepository _significantChangeProjectRepository = significantChangeProjectRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<PagedDataResponse<SignificantChangeProjectSearchResponse>> Handle(GetSignificantProjectsQuery query, CancellationToken cancellationToken)
		{
			var (projects, totalCount) = await _significantChangeProjectRepository.SearchSignificantChangeProjects(query.Page, query.Count, cancellationToken);

			var routeValues = new Dictionary<string, object?>();
			var pageResponse = PagingResponseFactory.Create("significant-change/significant-change-projects", query.Page, query.Count, totalCount, routeValues);

			var projectDtos = _mapper.Map<IEnumerable<SignificantChangeProjectDto>>(projects);
			var data = _mapper.Map<IEnumerable<SignificantChangeProjectSearchResponse>>(projectDtos);

			return new PagedDataResponse<SignificantChangeProjectSearchResponse>(data,
				pageResponse);
		}

	}
}
