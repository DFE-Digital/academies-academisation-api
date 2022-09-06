using Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate;

namespace Dfe.Academies.Academisation.IData.LegalRequirementAggregate
{
	public interface ILegalRequirementGetDataQuery
	{
		Task<LegalRequirement?> Execute(int id);
	}
}
