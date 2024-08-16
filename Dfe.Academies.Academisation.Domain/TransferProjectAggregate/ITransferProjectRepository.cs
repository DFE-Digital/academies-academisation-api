using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public interface ITransferProjectRepository : IRepository<TransferProject>, IGenericRepository<TransferProject>
	{
		public Task<ITransferProject?> GetByUrn(int urn);
		public Task<IEnumerable<ITransferProject?>> GetAllTransferProjects();
		public Task<IEnumerable<ITransferProject?>> GetIncompleteProjects();
		Task<IEnumerable<ITransferProject>> GetTransfersProjectsForGroup(string ukprn, CancellationToken cancellationToken);
		Task<IEnumerable<ITransferProject>> GetTransferProjectsByIdsAsync(List<int> ids, CancellationToken cancellationToken);
		Task<(IEnumerable<ITransferProject>, int totalcount)> SearchProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count);
		Task<IEnumerable<ITransferProject>> GetProjectsByProjectGroupIdAsync(int? projectGroupId, CancellationToken cancellationToken);
	}
}
