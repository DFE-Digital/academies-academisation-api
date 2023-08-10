using Dfe.Academies.Academisation.Domain.SeedWork;


namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public interface ITransferProjectRepository : IRepository<TransferProject>, IGenericRepository<TransferProject>
	{
		public Task<TransferProject?> GetByUrn(int urn);
	}
}
