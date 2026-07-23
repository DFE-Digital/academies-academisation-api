using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public interface ISignificantChangeProjectRepository : IRepository<SignificantChangeProject>, IGenericRepository<SignificantChangeProject>
	{
		Task<(IEnumerable<SignificantChangeProject> projects, int totalCount)> SearchSignificantProjects(int page, int count, CancellationToken cancellationToken);
		Task<SignificantChangeProject?> GetSignificantProjectById(int id, CancellationToken cancellationToken);
	}
}