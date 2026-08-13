using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public record GetSignificantProjectsQuery(
		int Page,
		int Count,
		string? Keyword = null,
		List<string>? Status = null,
		List<string>? Assignee = null,
		List<byte>? Tier = null,
		List<string>? Route = null) 
		: IRequest<PagedDataResponse<SignificantChangeProjectSearchResponse>>;
}
