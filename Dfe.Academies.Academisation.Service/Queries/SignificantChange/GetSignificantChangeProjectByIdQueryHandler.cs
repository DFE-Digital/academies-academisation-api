using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public class GetSignificantChangeProjectByIdQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository)
	: IRequestHandler<GetSignificantChangeProjectByIdQuery, SignificantChangeProjectSearchResponse?>
{
	public async Task<SignificantChangeProjectSearchResponse?> Handle(GetSignificantChangeProjectByIdQuery query,
		CancellationToken cancellationToken)
	{
		SignificantChangeProject? project =
			await significantChangeProjectRepository.GetSignificantChangeProjectById(query.Id, cancellationToken);

		return project?.MapToServiceModel();
	}
}