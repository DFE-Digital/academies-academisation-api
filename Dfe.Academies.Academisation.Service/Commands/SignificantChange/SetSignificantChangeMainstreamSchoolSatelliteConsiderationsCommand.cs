using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

/// <summary>Updates the mainstream school satellite considerations for a project.</summary>
public sealed record SetSignificantChangeMainstreamSchoolSatelliteConsiderationsPublicCommand(
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink);

public sealed record SetSignificantChangeMainstreamSchoolSatelliteConsiderationsCommand(
	int Id,
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink) : IRequest<CommandResult>;
