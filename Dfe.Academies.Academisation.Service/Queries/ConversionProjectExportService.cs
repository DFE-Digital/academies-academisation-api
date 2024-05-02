using ClosedXML.Excel;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ConversionProjectExportService : IConversionProjectExportService
	{
		private readonly IConversionProjectQueryService _conversionProjectQueryService;
		private readonly IAdvisoryBoardDecisionQueryService _advisoryBoardDecisionQueryService;

		public ConversionProjectExportService(
			IConversionProjectQueryService conversionProjectQueryService,
			IAdvisoryBoardDecisionQueryService advisoryBoardDecisionQueryService
			)
		{
			_conversionProjectQueryService = conversionProjectQueryService;
			_advisoryBoardDecisionQueryService = advisoryBoardDecisionQueryService;
		}

		public async Task<Stream?> ExportProjectsToSpreadsheet(ConversionProjectSearchModel searchModel)
		{
			var result = await _conversionProjectQueryService.GetProjectsV2(searchModel.StatusQueryString, searchModel.TitleFilter,
				searchModel.DeliveryOfficerQueryString, 1, int.MaxValue, searchModel.RegionQueryString, searchModel.LocalAuthoritiesQueryString, searchModel.AdvisoryBoardDatesQueryString);

			if (result?.Data == null || !result.Data.Any())
			{
				return null;
			}

			var projects = result.Data;
			return await GenerateSpreadsheet(projects);
		}

		private async Task<Stream> GenerateSpreadsheet(IEnumerable<ConversionProjectServiceModel> projects)
		{
			var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Projects");

			// Add headers and set styles
			worksheet.Cell(1, 1).Value = "School";
			worksheet.Cell(1, 2).Value = "URN";
			worksheet.Cell(1, 3).Value = "School Type";
			worksheet.Cell(1, 4).Value = "School Phase";
			worksheet.Cell(1, 5).Value = "Incoming Trust";
			worksheet.Cell(1, 6).Value = "Incoming Trust Reference Number";
			worksheet.Cell(1, 7).Value = "Local Authority";
			worksheet.Cell(1, 8).Value = "Region";
			worksheet.Cell(1, 9).Value = "Advisory Board Date";
			worksheet.Cell(1, 10).Value = "Decision Date";
			worksheet.Cell(1, 11).Value = "Status";
			worksheet.Cell(1, 12).Value = "Assigned To";
			worksheet.Cell(1, 13).Value = "Academy Type and Route";
			worksheet.Cell(1, 14).Value = "Part of PFI scheme";
			worksheet.Cell(1, 15).Value = "Date AO issued";
			worksheet.Cell(1, 16).Value = "Date dAO issued";

			// Bold headers
			for (int col = 1; col <= 16; col++)
			{
				worksheet.Cell(1, col).Style.Font.Bold = true;
			}

			int row = 2;
			foreach (var project in projects)
			{
				worksheet.Cell(row, 1).Value = project.SchoolName;
				worksheet.Cell(row, 2).Value = project.Urn;
				worksheet.Cell(row, 3).Value = project.SchoolType;
				worksheet.Cell(row, 4).Value = project.SchoolPhase;
				worksheet.Cell(row, 5).Value = project.NameOfTrust;
				worksheet.Cell(row, 6).Value = project.TrustReferenceNumber;
				worksheet.Cell(row, 7).Value = project.LocalAuthority;
				worksheet.Cell(row, 8).Value = project.Region;
				worksheet.Cell(row, 9).Value = project.HeadTeacherBoardDate;

				var advisoryBoardDecision = await _advisoryBoardDecisionQueryService.GetByProjectId(project.Id);
				worksheet.Cell(row, 10).Value = advisoryBoardDecision?.AdvisoryBoardDecisionDate;
				worksheet.Cell(row, 11).Value = project.ProjectStatus;
				worksheet.Cell(row, 12).Value = project.AssignedUser?.FullName;
				worksheet.Cell(row, 13).Value = project.AcademyTypeAndRoute;
				worksheet.Cell(row, 14).Value = project.PartOfPfiScheme;

				if (project.AcademyTypeAndRoute?.ToLower().Equals("sponsored") ?? false)
				{
					worksheet.Cell(row, 16).Value = project.DaoPackSentDate;
				}
				else
				{
					worksheet.Cell(row, 15).Value = advisoryBoardDecision?.AcademyOrderDate;
				}
				row++;
			}

			worksheet.Columns().AdjustToContents();
			var stream = new MemoryStream();
			workbook.SaveAs(stream);
			stream.Position = 0;

			return stream;
		}

	}
}
