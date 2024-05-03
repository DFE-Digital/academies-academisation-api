using ClosedXML.Excel;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.Service.Queries
{
	internal record SpreadsheetFields(string header, string value);

	public class TransferProjectExportService : ITransferProjectExportService
	{
		private readonly ITransferProjectQueryService _transferProjectQueryService;

		public TransferProjectExportService(ITransferProjectQueryService transferProjectQueryService)
		{
			_transferProjectQueryService = transferProjectQueryService;
		}

		public async Task<Stream?> ExportTransferProjectsToSpreadsheet(GetProjectSearchModel searchModel)
		{
			var result = await _transferProjectQueryService.GetExportedTransferProjects(searchModel.StatusQueryString, searchModel.TitleFilter, searchModel.DeliveryOfficerQueryString, searchModel.Page, searchModel.Count);

			if (result?.Results == null || !result.Results.Any())
			{
				return null;
			}

			var projects = result.Results;
			return await GenerateSpreadsheet(projects);
		}

		private static Task<Stream> GenerateSpreadsheet(IEnumerable<ExportedTransferProjectModel> projects)
		{
			string[] headers = new[]{
				"School", "URN", "School Type", "Incoming Trust", "Outgoing Trust", "Incoming Trust UKPRN", "Local Authority", "Region",
				"Advisory Board Date", "Decision Date", "Status", "Assigned To", "Reason for transfer", "Type of transfer",
				"Proposed academy transfer date"
			};

			var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Projects");

			for (int i = 0; i < headers.Length; i++)
			{
				worksheet.Cell(1, i + 1).Value = headers[i];
				worksheet.Cell(1, i + 1).Style.Font.Bold = true;
			}

			int row = 2;
			foreach (var project in projects)
			{
				worksheet.Cell(row, 1).Value = project.SchoolName;
				worksheet.Cell(row, 2).Value = project.Urn;
				worksheet.Cell(row, 3).Value = project.SchoolType;
				worksheet.Cell(row, 4).Value = project.IncomingTrustName;
				worksheet.Cell(row, 5).Value = project.OutgoingTrustName;
				worksheet.Cell(row, 6).Value = project.IncomingTrustUkprn;
				worksheet.Cell(row, 7).Value = project.LocalAuthority;
				worksheet.Cell(row, 8).Value = project.Region;
				worksheet.Cell(row, 9).Value = project.AdvisoryBoardDate;
				worksheet.Cell(row, 10).Value = project.DecisionDate;
				worksheet.Cell(row, 11).Value = project.Status;
				worksheet.Cell(row, 12).Value = project.AssignedUserFullName;
				worksheet.Cell(row, 13).Value = project.TransferReason;
				worksheet.Cell(row, 14).Value = project.TransferType;
				worksheet.Cell(row, 15).Value = project.ProposedAcademyTransferDate;
				row++;
			}
			worksheet.Columns().AdjustToContents();
			var stream = new MemoryStream();
			workbook.SaveAs(stream);
			stream.Position = 0;

			return Task.FromResult<Stream>(stream);
		}
	}
}
