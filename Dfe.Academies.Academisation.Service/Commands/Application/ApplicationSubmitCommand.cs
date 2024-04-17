using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public record ApplicationSubmitCommand(
		int applicationId) : IRequest<CommandOrCreateResult>;
}
