using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Factories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public class GetSignificantProjectsQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository, IDateTimeProvider dateTimeProvider, ILogger<GetSignificantProjectsQueryHandler> logger) : IRequestHandler<GetSignificantProjectsQuery, PagedDataResponse<SignificantChangeProjectResponse>>
	{
		private readonly ISignificantChangeProjectRepository _significantChangeProjectRepository = significantChangeProjectRepository;

		public async Task<PagedDataResponse<SignificantChangeProjectResponse>> Handle(GetSignificantProjectsQuery query, CancellationToken cancellationToken)
		{
		var (projects, totalCount) = await _significantChangeProjectRepository.SearchSignificantProjects(query.Page, query.Count, cancellationToken);

		var routeValues = new Dictionary<string, object?>();
		var pageResponse = PagingResponseFactory.Create("significant-change/significant-change-projects", query.Page, query.Count, totalCount, routeValues);
	
		var data = projects.Select(p => new SignificantChangeProjectResponse
		{
			Id = p.Id,
			Urn = p.Urn,
			Tier = p.Tier,
			TrustName = p.TrustName,
			TrustUkprn = p.TrustUkprn,
			TypeOfSignificantChange = p.TypeOfSignificantChange,
			Status = p.Status.ToString()
		});

		return new PagedDataResponse<SignificantChangeProjectResponse>(data,
			pageResponse);
		}

	}
}
