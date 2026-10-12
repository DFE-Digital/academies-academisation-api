using AutoMapper;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public class GetSignificantChangeProjectByIdQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository, IMapper mapper)
	: IRequestHandler<GetSignificantChangeProjectByIdQuery, SignificantChangeProjectGetByIdResponse?>
{

	public async Task<SignificantChangeProjectGetByIdResponse?> Handle(GetSignificantChangeProjectByIdQuery query,
		CancellationToken cancellationToken)
	{
		SignificantChangeProject? project =
			await significantChangeProjectRepository.GetSignificantChangeProjectById(query.Id, cancellationToken);

		if (project is null)
		{
			return null;
		}

		SignificantChangeProjectDto projectDto = mapper.Map<SignificantChangeProjectDto>(project);

		return mapper.Map<SignificantChangeProjectGetByIdResponse>(projectDto);
	}
}
