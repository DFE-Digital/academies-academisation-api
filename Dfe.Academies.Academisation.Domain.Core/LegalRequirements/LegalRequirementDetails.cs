namespace Dfe.Academies.Academisation.Domain.Core.LegalRequirements
{
	public record LegalRequirementDetails(
		int ProjectId,
		YesNoState? HaveProvidedResolution,
		YesNoState? HadConsultation,
		YesNoState? HasDioceseConsented,
		YesNoState? HasFoundationConsented,
		bool IsSectionComplete
	);
}
