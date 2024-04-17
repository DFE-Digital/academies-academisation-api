using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ITransferProjectQueryService
	{
		Task<AcademyTransferProjectResponse?> GetByUrn(int Urn);
		Task<AcademyTransferProjectResponse?> GetById(int id);
		Task<PagedResultResponse<AcademyTransferProjectSummaryResponse>> GetTransferProjects(int page, int count, int? urn,
		string title);
		Task<PagedResultResponse<ExportedTransferProjectModel>> GetExportedTransferProjects(string? title);

	}
}
