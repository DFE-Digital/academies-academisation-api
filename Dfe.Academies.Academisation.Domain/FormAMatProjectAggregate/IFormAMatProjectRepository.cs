using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate
{
	public interface IFormAMatProjectRepository : IRepository<FormAMatProject>, IGenericRepository<FormAMatProject>
	{
		Task<IFormAMatProject?> GetByApplicationReference(string? applicationReferenceNumber, CancellationToken cancellationToken);
		Task<List<IFormAMatProject>> GetByIds(IEnumerable<int?> formAMatProjectIds, CancellationToken cancellationToken);
		Task<List<IFormAMatProject>> GetProjectsWithoutReference(CancellationToken cancellationToken);
		Task<IEnumerable<FormAMatProject>> SearchProjectsByTermAsync(string searchTerm, CancellationToken cancellationToken);

		Task CreateFormAMatProjectWithTrustReferenceNumber(IFormAMatProject formAMatProject);
	}
}
