using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public record SetSignificantChangeHqtfConsiderationPublicCommand(
	HqtfConsiderationAnswer? HqtfConsiderationAnswer,
	string? AdditionalInformation,
	string? SupportingEvidence);

public record SetSignificantChangeHqtfConsiderationCommand(
	int Id,
	HqtfConsiderationAnswer? HqtfConsiderationAnswer,
	string? AdditionalInformation,
	string? SupportingEvidence)
	: SetSignificantChangeHqtfConsiderationPublicCommand(HqtfConsiderationAnswer, AdditionalInformation, SupportingEvidence),
		IRequest<CommandResult>;
