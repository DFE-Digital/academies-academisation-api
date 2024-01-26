using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate
{
	public interface IFormAMatProjectRepository : IRepository<FormAMatProject>, IGenericRepository<FormAMatProject>
	{
		Task<FormAMatProject> GetByApplicationReference(string? applicationReferenceNumber, CancellationToken cancellationToken);
	}
}
