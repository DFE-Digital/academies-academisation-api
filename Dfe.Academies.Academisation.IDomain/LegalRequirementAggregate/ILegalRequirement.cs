using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.LegalRequirements;

namespace Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate
{
	public interface LegalRequirement
	{
		int Id { get; }
		DateTime CreatedOn { get; }
		DateTime LastModifiedOn { get; }
		LegalRequirementDetails LegalRequirementDetails { get; }		
	}
}
