using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface ITransferProjectExportService
	{
		Task<Stream?> ExportTransferProjectsToSpreadsheet(TransferProjectSearchModel searchModel);
	}
}
