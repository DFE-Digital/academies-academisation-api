using Dfe.Academies.Academisation.Domain.SignificantChange;

namespace Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;

/// <summary>Represents whether a project's proposal conflicts with a Local SEN Reform Plan.</summary>
public sealed record SignificantChangeLocalSenPlansResponse
{
	public YesNoNotApplicableAnswer? Answer { get; init; }
	public string? AdditionalInformation { get; init; }
	public string? SupportingEvidence { get; init; }
	public string Status { get; init; } = string.Empty;
}
