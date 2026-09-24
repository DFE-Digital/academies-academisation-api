using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangePlanningPermissionPublicCommand(
		PlanningPermissionAnswer PlanningPermissionAnswer,
		string? AdditionalInformation,
		string? SupportingEvidence);

	public record SetSignificantChangePlanningPermissionCommand(
		int Id,
		PlanningPermissionAnswer PlanningPermissionAnswer,
		string? AdditionalInformation,
		string? SupportingEvidence)
		: SetSignificantChangePlanningPermissionPublicCommand(PlanningPermissionAnswer, AdditionalInformation, SupportingEvidence), IRequest<CommandResult>;
}
