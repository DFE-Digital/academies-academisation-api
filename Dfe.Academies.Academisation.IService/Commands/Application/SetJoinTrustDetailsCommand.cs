using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.IService.Commands.Application
{
	public record SetJoinTrustDetailsCommand(
		int applicationId,
		string trustName,
		int UKPRN,
		ChangesToTrust? changesToTrust,
		string? changesToTrustExplained,
		bool? changesToLaGovernance,
		string? changesToLaGovernanceExplained) : IRequest<CommandResult>;
}
