using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision
{
	public record SignificantChangeStatusCommand(int Id, Decision Status, bool? ApprovedWithConditions) : IRequest<CommandResult>;
}
