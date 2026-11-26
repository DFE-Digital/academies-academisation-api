using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

/// <summary>Updates the impact on SEN provision answer for a project.</summary>
public sealed record SetSignificantChangeImpactOnSenProvisionPublicCommand(
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink);

public sealed record SetSignificantChangeImpactOnSenProvisionCommand(
	int Id,
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink) : IRequest<CommandResult>;
