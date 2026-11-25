using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

/// <summary>Updates the local SEN plans answer for a project.</summary>
public sealed record SetSignificantChangeLocalSenPlansPublicCommand(
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink);

public sealed record SetSignificantChangeLocalSenPlansCommand(
	int Id,
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink) : IRequest<CommandResult>;
