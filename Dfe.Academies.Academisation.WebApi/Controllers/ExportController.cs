using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	[Route("export/")]
	[ApiController]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class ExportController : ControllerBase
	{
		private readonly IConversionProjectExportService _conversionProjectExportService;
		public ExportController(IConversionProjectExportService conversionProjectExportService)
		{
			_conversionProjectExportService = conversionProjectExportService;
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
	}
}
