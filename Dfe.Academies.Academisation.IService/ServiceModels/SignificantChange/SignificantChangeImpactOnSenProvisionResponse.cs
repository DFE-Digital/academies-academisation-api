using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

/// <summary>Represents whether a project's proposal is likely to improve the standard, quality and range of SEN provision.</summary>
public sealed record SignificantChangeImpactOnSenProvisionResponse
{
	public YesNoNotApplicableAnswer? Answer { get; init; }
	public string? AdditionalInformation { get; init; }
	public string? SupportingEvidence { get; init; }
	public string Status { get; init; } = string.Empty;
}
