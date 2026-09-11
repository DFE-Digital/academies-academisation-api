using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public record GetSignificantChangeDecisionQuery(
		int ProjectId)
		: IRequest<SignificantChangeDecisionServiceModel?>;
}
