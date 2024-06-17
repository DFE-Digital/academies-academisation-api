using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public record SetJoinTrustDetailsCommand(
		int applicationId,
		string trustName,
		string trustReference,
		int UKPRN,
		ChangesToTrust? changesToTrust,
		string? changesToTrustExplained,
		bool? changesToLaGovernance,
		string? changesToLaGovernanceExplained) : IRequest<CommandResult>;
}
