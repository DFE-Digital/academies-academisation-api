using AutoMapper;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public class GetSignificantChangeDecisionQueryHandler(IAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository) : IRequestHandler<GetSignificantChangeDecisionQuery, SignificantChangeDecisionServiceModel?>
	{
		public async Task<SignificantChangeDecisionServiceModel?> Handle(GetSignificantChangeDecisionQuery query, CancellationToken cancellationToken)
		{
			var decision = await advisoryBoardDecisionRepository.GetSignificantChangeDecision(query.ProjectId);

			return decision?.MapFromDomain();
		}
	}
}
