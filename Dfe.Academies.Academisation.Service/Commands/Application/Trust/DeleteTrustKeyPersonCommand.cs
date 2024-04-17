using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public record DeleteTrustKeyPersonCommand(
		int ApplicationId,
		int KeyPersonId) : IRequest<CommandResult>;
}
