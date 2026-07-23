using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public class GetSignificantProjectByIdQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository)
	: IRequestHandler<GetSignificantProjectByIdQuery, SignificantChangeProjectSearchResponse?>
{
	public async Task<SignificantChangeProjectSearchResponse?> Handle(GetSignificantProjectByIdQuery query,
		CancellationToken cancellationToken)
	{
		SignificantChangeProject? project =
			await significantChangeProjectRepository.GetSignificantProjectById(query.Id, cancellationToken);

		return project?.MapToServiceModel();
	}
}