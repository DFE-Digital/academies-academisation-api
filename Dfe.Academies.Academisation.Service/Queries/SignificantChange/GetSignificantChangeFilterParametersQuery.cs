
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Queries.SignificantChange
{
	public record GetSignificantChangeFilterParametersQuery : IRequest<SignificantChangeFilterParameters>;
}