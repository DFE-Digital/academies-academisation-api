using Dfe.Academies.Academisation.IService.ServiceModels.Summary;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ISummaryQueryService
	{
		Task<IEnumerable<ProjectSummary>> GetProjectSummariesByAssignedEmail(string email, bool includeConversions, bool includeTransfers, bool includeFormAMat, CancellationToken cancellationToken);
	}
}
