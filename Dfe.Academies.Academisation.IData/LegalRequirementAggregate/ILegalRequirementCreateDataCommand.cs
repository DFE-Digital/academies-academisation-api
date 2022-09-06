using Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate;

namespace Dfe.Academies.Academisation.IData.LegalRequirementAggregate
{
	public interface ILegalRequirementCreateDataCommand
	{
		Task Execute(LegalRequirement legalRequirement);
	}
}
