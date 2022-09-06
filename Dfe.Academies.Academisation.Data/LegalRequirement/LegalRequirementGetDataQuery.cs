using Dfe.Academies.Academisation.IData.LegalRequirementAggregate;
using Dfe.Academies.Academisation.IDomain.LegalRequirementAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.LegalRequirement
{
	public class LegalRequirementGetDataQuery : ILegalRequirementGetDataQuery
	{
		private readonly AcademisationContext _context;

		public LegalRequirementGetDataQuery(AcademisationContext context)
		{
			_context = context;
		}

		public async Task<IDomain.LegalRequirementAggregate.LegalRequirement?> Execute(int id)
		{
			var toReturn = await _context.LegalRequirements
				.SingleOrDefaultAsync(lr => lr.ProjectId == id);

			return toReturn?.MapToDomain();
		}
	}
}
