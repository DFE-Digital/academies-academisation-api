using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.LegalRequirements;
using Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate;

namespace Dfe.Academies.Academisation.Domain.LegalRequirement
{
	public class LegalRequirement : IDomain.LegalRequirementAggregate.LegalRequirement
	{
		public LegalRequirement(int id, DateTime createdOn, DateTime lastModifiedOn, LegalRequirementDetails legalRequirementDetails)
		{
			Id = id;
			CreatedOn = createdOn;
			LastModifiedOn = lastModifiedOn;
			LegalRequirementDetails = legalRequirementDetails;
		}

		public int Id { get; private set; }
		public DateTime CreatedOn { get; }
		public DateTime LastModifiedOn { get; }
		public LegalRequirementDetails LegalRequirementDetails { get; private set; }		
	}
}
