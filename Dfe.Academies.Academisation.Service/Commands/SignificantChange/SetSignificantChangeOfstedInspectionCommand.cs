using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public record SetSignificantChangeOfstedInspectionPublicCommand(
		DateTime? InspectionDate,
		bool? MeetsOfstedStandards,
		string? AdditionalInformation,
		string? SupportingEvidence);

	public record SetSignificantChangeOfstedInspectionCommand(
		int Id,
		DateTime? InspectionDate,
		bool? MeetsOfstedStandards,
		string? AdditionalInformation,
		string? SupportingEvidence) : IRequest<CommandResult>;
}
