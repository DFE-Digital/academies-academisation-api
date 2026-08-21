using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public interface ISignificantChangeProjectRepository : IRepository<SignificantChangeProject>,
		IGenericRepository<SignificantChangeProject>
	{
		Task<(IEnumerable<SignificantChangeProject> projects, int totalCount)> SearchSignificantChangeProjects(int page,
			int count, string? keyword, List<string>? status, List<string>? assignee, List<byte>? tier,
			List<string>? route, CancellationToken cancellationToken);

		Task<SignificantChangeProject?> GetSignificantChangeProjectById(int id, CancellationToken cancellationToken);
		Task<SignificantChangeFilterParameters> GetFilterParameters(CancellationToken cancellationToken);
	}
}
