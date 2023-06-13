using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public record DeleteTrustKeyPersonCommand(
		int ApplicationId,
		int KeyPersonId) : IRequest<CommandResult>;
}
