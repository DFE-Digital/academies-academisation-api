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
		public async Task<PagedDataResponse<SignificantChangeProjectSearchResponse>> Handle(GetSignificantProjectsQuery query, CancellationToken cancellationToken)
		{
			var searchOptions = new SignificantChangeProjectSearchOptions
			{
				Page = query.Page,
				Count = query.Count,
				Keyword = query.Keyword,
				Status = query.Status,
				Assignee = query.Assignee,
				Tier = query.Tier,
				Route = query.Route,
				LocalAuthorities = query.LocalAuthority,
				Regions = query.Region
			};

			var (projects, totalCount) = await significantChangeProjectRepository.SearchSignificantChangeProjects(searchOptions, cancellationToken);

			var routeValues = new Dictionary<string, object?>();
			var pageResponse = PagingResponseFactory.Create("significant-change/significant-change-projects", query.Page, query.Count, totalCount, routeValues);

			var projectDtos = mapper.Map<IEnumerable<SignificantChangeProjectDto>>(projects);
			var data = mapper.Map<IEnumerable<SignificantChangeProjectSearchResponse>>(projectDtos);

			return new PagedDataResponse<SignificantChangeProjectSearchResponse>(data,
				pageResponse);
		}

	}
}
