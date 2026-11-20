using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

/// <summary>Represents a project's trust experience and governance.</summary>
public sealed record SignificantChangeTrustExperienceAndGovernanceResponse
{
	public YesNoNotApplicableAnswer? Answer { get; init; }
	public string? AdditionalInformation { get; init; }
	public string? SupportingEvidence { get; init; }
	public string Status { get; init; } = string.Empty;
}
