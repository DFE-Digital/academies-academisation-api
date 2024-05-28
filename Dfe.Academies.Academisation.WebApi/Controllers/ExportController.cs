using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("export/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ExportController : ControllerBase
	{
		private readonly IConversionProjectExportService _conversionProjectExportService;
		private readonly ITransferProjectExportService _transferProjectExportService;
		public ExportController(
			IConversionProjectExportService conversionProjectExportService,
			ITransferProjectExportService transferProjectExportService
		)
		{
			_conversionProjectExportService = conversionProjectExportService;
			_transferProjectExportService = transferProjectExportService;
		}

		[HttpPost("export-projects", Name = "ExportProjectsToSpreadsheet")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ExportProjectsToSpreadsheet(ConversionProjectSearchModel searchModel)
		{
			var spreadsheetStream = await _conversionProjectExportService.ExportProjectsToSpreadsheet(searchModel);
			if (spreadsheetStream == null)
			{
				return NotFound("No projects found for the specified criteria.");
			}

			return File(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "exported_projects.xlsx");
		}

		[HttpPost("export-transfer-projects", Name = "ExportTransferProjectsToSpreadsheet")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> ExportTransferProjectsToSpreadsheet(GetProjectSearchModel searchModel)
		{
			var spreadsheetStream = await _transferProjectExportService.ExportTransferProjectsToSpreadsheet(searchModel);
			if (spreadsheetStream == null)
			{
				return NotFound("No projects found for the specified criteria.");
			}

			return File(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "exported_projects.xlsx");
		}
	}
}
