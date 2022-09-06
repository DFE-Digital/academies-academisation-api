using Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate;
namespace Dfe.Academies.Academisation.IData.LegalRequirementAggregate
{
	public interface ILegalRequirementUpdateDataCommand
	{
		Task Execute(LegalRequirement legalRequirement);
	}
}
