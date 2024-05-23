using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface ITransferProjectExportService
	{
		Task<Stream?> ExportTransferProjectsToSpreadsheet(GetProjectSearchModel searchModel);
	}
}
