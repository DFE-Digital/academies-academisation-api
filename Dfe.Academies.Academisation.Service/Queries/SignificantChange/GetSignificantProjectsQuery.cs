using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public record GetSignificantProjectsQuery(
		int Page,
		int Count)
		: IRequest<PagedDataResponse<SignificantChangeProjectResponse>>;
}
