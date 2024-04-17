using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public record UpdateTrustKeyPersonCommand(
		int ApplicationId,
		int KeyPersonId,
		IEnumerable<TrustKeyPersonRoleServiceModel> Roles,
		string Name,
		DateTime DateOfBirth,
		string Biography) : IRequest<CommandResult>;
}
