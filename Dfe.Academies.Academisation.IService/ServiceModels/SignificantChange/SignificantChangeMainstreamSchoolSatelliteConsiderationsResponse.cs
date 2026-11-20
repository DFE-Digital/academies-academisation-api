using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

/// <summary>Represents a project's mainstream school satellite considerations.</summary>
public sealed record SignificantChangeMainstreamSchoolSatelliteConsiderationsResponse
{
	public YesNoNotApplicableAnswer? Answer { get; init; }
	public string? AdditionalInformation { get; init; }
	public string? SupportingEvidence { get; init; }
	public string Status { get; init; } = string.Empty;
}
