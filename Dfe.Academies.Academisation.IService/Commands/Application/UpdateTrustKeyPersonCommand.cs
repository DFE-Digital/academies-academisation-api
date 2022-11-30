using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public record UpdateTrustKeyPersonCommand(
		int ApplicationId,
		int KeyPersonId,
		KeyPersonRole Role,
		string TimeInRole,
		int PersonId,
		string FirstName,
		string Surname,
		string? ContactEmailAddress,
		DateTime? DateOfBirth,
		string Biography) : IRequest<CommandResult>;
}
