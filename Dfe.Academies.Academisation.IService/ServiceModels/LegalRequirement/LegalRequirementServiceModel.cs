using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement
{
	public class LegalRequirementServiceModel
	{
		public int Id { get; init; }
		public int ProjectId { get; init; }
		public YesNoState? HaveProvidedResolution { get; init; }
		public YesNoState? HadConsultation { get; init; }
		public YesNoState? HasDioceseConsented { get; init; }
		public YesNoState? HasFoundationConsented { get; init; }
		public bool IsSectionComplete { get; init; }

	}
}
