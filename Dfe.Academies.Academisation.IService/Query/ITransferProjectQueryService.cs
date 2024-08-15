using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ITransferProjectQueryService
	{
		Task<AcademyTransferProjectResponse?> GetByUrn(int Urn);
		Task<AcademyTransferProjectResponse?> GetById(int id);
		Task<IEnumerable<AcademyTransferProjectSummaryResponse>?> GetTransferProjectsByIncomingTrustUkprn(string ukprn, CancellationToken cancellationToken);
		Task<PagedResultResponse<AcademyTransferProjectSummaryResponse>> GetTransferProjects(int page, int count, int? urn,
		string title);
		Task<PagedDataResponse<AcademyTransferProjectSummaryResponse>?> GetProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count);
		Task<PagedResultResponse<ExportedTransferProjectModel>> GetExportedTransferProjects(IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count);

	}
}
