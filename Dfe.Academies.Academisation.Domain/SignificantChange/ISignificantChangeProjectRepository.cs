using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.SignificantChange
{
	public record SignificantChangeProjectSearchOptions
	{
		public required int Page { get; init; }
		public required int Count { get; init; }
		public string? Keyword { get; init; }
		public List<string>? Status { get; init; }
		public List<string>? Assignee { get; init; }
		public List<byte>? Tier { get; init; }
		public List<string>? Route { get; init; }
		public List<string>? LocalAuthorities { get; init; }
		public List<string>? Regions { get; init; }
	}

	public interface ISignificantChangeProjectRepository : IRepository<SignificantChangeProject>,
		IGenericRepository<SignificantChangeProject>
	{
		Task<(IEnumerable<SignificantChangeProject> projects, int totalCount)> SearchSignificantChangeProjects(
			SignificantChangeProjectSearchOptions arguments,
			CancellationToken cancellationToken);

		Task<SignificantChangeProject?> GetSignificantChangeProjectById(int id, CancellationToken cancellationToken);
		Task<SignificantChangeFilterParameters> GetFilterParameters(CancellationToken cancellationToken);
        Task<List<SignificantChangeProject>> GetProjectsToSendToCompleteAsync(CancellationToken cancellationToken);
    }
}
