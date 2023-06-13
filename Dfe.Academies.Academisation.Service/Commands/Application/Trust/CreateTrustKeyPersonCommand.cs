using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public record CreateTrustKeyPersonCommand(
		int ApplicationId,
		IEnumerable<TrustKeyPersonRoleServiceModel> Roles,
		string Name,
		DateTime DateOfBirth,
		string Biography) : IRequest<CommandResult>;
}
