using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public record AddTrustKeyPersonCommand(
		int ApplicationId,
		KeyPersonRole Role,
		string TimeInRole,
		string FirstName,
		string Surname,
		string? ContactEmailAddress,
		DateTime? DateOfBirth,
		string Biography) : IRequest<CommandResult>;
}
