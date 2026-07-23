using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange;

public record GetSignificantProjectByIdQuery(int Id) : IRequest<SignificantChangeProjectSearchResponse?>;