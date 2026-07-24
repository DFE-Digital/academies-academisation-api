using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record CreateSignificantProjectCommand(int Urn, byte Tier, string Route, string TrustUkprn)
		: IRequest<CreateResult>;
}
