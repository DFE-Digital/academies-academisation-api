using Dfe.Academies.Academisation.IService.ServiceModels;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface ITransferProjectExportService
	{
		Task<Stream?> ExportTransferProjectsToSpreadsheet(GetProjectSearchModel searchModel);
	}
}
