using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

/// <summary>The satellite site arrangement assessment and its task status.</summary>
public sealed record SignificantChangeSatelliteSiteArrangementResponse
{
	public YesNoNotApplicableAnswer? Answer { get; init; }
	public string? AdditionalInformation { get; init; }
	public string? SupportingEvidence { get; init; }
	public string Status { get; init; } = string.Empty;
}
