using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.Core;

namespace Dfe.Academies.Academisation.Data.ConversionLegalRequirement
{
	[Table(name: "LegalRequirement")]
	public class LegalRequirementState : BaseEntity
	{		
		public int ProjectId { get; set; }
		public YesNoState? HaveProvidedResolution { get; set; }
		public YesNoState? HadConsultation { get; set; }
		public YesNoState? HasDioceseConsented { get; set; }
		public YesNoState? HasFoundationConsented { get; set; }
		public bool IsSectionComplete { get; set; }
	}
}
