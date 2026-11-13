using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

/// <summary>Sets the satellite site arrangement assessment for a significant change project.</summary>
public sealed record SetSignificantChangeSatelliteSiteArrangementPublicCommand(
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidence);

public sealed record SetSignificantChangeSatelliteSiteArrangementCommand(
	int Id,
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidence) : IRequest<CommandResult>;
