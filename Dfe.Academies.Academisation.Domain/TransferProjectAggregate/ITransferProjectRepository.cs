using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public interface ITransferProjectRepository : IRepository<TransferProject>, IGenericRepository<TransferProject>
	{
		public Task<ITransferProject?> GetByUrn(int urn);
		public Task<IEnumerable<ITransferProject?>> GetAllTransferProjects();
		public Task<IEnumerable<ITransferProject?>> GetAllTransferProjectsWhereTrustNameIsNull();
	}
}
