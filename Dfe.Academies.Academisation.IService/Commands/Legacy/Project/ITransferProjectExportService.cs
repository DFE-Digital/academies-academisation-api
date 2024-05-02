using Dfe.Academies.Academisation.Data.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface ITransferProjectExportService
	{
		Task<Stream?> ExportTransferProjectsToSpreadsheet(GetProjectSearchModel searchModel);
	}
}
