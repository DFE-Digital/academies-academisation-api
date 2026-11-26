using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange;

/// <summary>Updates the trust experience and governance for a project.</summary>
public sealed record SetSignificantChangeTrustExperienceAndGovernancePublicCommand(
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink);

public sealed record SetSignificantChangeTrustExperienceAndGovernanceCommand(
	int Id,
	YesNoNotApplicableAnswer? Answer,
	string? AdditionalInformation,
	string? SupportingEvidenceSharepointLink) : IRequest<CommandResult>;
