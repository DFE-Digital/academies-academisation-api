using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public record DeleteSchoolCommand(
		int ApplicationId,
		int Urn) : IRequest<CommandResult>;
}
