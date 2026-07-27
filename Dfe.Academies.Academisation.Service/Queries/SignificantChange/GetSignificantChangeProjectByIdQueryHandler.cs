using AutoMapper;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public class GetSignificantChangeProjectByIdQueryHandler(ISignificantChangeProjectRepository significantChangeProjectRepository, IMapper mapper)
	: IRequestHandler<GetSignificantChangeProjectByIdQuery, SignificantChangeProjectSearchResponse?>
{
	private readonly ISignificantChangeProjectRepository _significantChangeProjectRepository = significantChangeProjectRepository;
	private readonly IMapper _mapper = mapper;

	public async Task<SignificantChangeProjectSearchResponse?> Handle(GetSignificantChangeProjectByIdQuery query,
		CancellationToken cancellationToken)
	{
		SignificantChangeProject? project =
			await _significantChangeProjectRepository.GetSignificantChangeProjectById(query.Id, cancellationToken);

		if (project is null)
		{
			return null;
		}

		SignificantChangeProjectDto projectDto = _mapper.Map<SignificantChangeProjectDto>(project);

		return _mapper.Map<SignificantChangeProjectSearchResponse>(projectDto);
	}
}