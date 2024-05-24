using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IConversionProjectExportService
	{
		Task<Stream?> ExportProjectsToSpreadsheet(ConversionProjectSearchModel searchModel);
	}
}
