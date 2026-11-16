using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

public record SetSignificantChangeSatelliteSiteArrangementPublicCommand(
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidence);

public record SetSignificantChangeSatelliteSiteArrangementCommand(
	int Id,
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidence) : IRequest<CommandResult>;
