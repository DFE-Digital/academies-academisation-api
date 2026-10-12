using AutoMapper;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public class GetSignificantChangeProjectByIdQueryHandler(
	ISignificantChangeProjectRepository significantChangeProjectRepository,
	IMapper mapper,
	ISender sender
	)
	: IRequestHandler<GetSignificantChangeProjectByIdQuery, SignificantChangeProjectGetByIdResponse?>
{

	public async Task<SignificantChangeProjectGetByIdResponse?> Handle(GetSignificantChangeProjectByIdQuery query,
		CancellationToken cancellationToken)
	{
		SignificantChangeProject? project = await significantChangeProjectRepository
			.GetSignificantChangeProjectById(query.Id, cancellationToken);

        if (project is null) return null;

        SignificantChangeProjectDto projectDto = mapper.Map<SignificantChangeProjectDto>(project);

        var response = mapper.Map<SignificantChangeProjectGetByIdResponse>(projectDto);

        var decision = await sender.Send(
            new GetSignificantChangeDecisionQuery(query.Id),
            cancellationToken);

        response.Decision = decision?.Decision;
        response.DecisionDate = decision?.DecisionDate;

        return response;
	}
}
