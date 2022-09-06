using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;
using Dfe.Academies.Academisation.Service.Mappers.LegalRequirement;
using Dfe.Academies.Academisation.IData.LegalRequirementAggregate;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class LegalRequirementGetQuery : ILegalRequirementGetQuery
	{
		private readonly ILegalRequirementGetDataQuery _legalRequirementGetQuery;

		public LegalRequirementGetQuery(ILegalRequirementGetDataQuery legalRequirementGetQuery)
		{
			_legalRequirementGetQuery = legalRequirementGetQuery;
		}

		public async Task<LegalRequirementServiceModel?> Execute(int projectId)
		{
			var decision = await _legalRequirementGetQuery.Execute(projectId);
			return decision?.MapFromDomain();
		}
	}
}
