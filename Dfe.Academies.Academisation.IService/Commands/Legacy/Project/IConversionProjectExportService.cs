using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface IConversionProjectExportService
	{
		Task<Stream?> ExportProjectsToSpreadsheet(ConversionProjectSearchModel searchModel);
	}
}
