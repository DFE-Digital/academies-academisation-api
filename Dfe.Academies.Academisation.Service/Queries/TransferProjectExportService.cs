using ClosedXML.Excel;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.Service.Queries
{
	internal record SpreadsheetFields(string header, string value);

	public class TransferProjectExportService(ITransferProjectQueryService transferProjectQueryService, IAdvisoryBoardDecisionQueryService advisoryBoardDecisionQueryService) : ITransferProjectExportService
	{
		public async Task<Stream?> ExportTransferProjectsToSpreadsheet(GetProjectSearchModel searchModel)
		{
			var result = await transferProjectQueryService.GetExportedTransferProjects(searchModel.StatusQueryString, searchModel.TitleFilter, searchModel.DeliveryOfficerQueryString, searchModel.Page, int.MaxValue);

			if (result?.Results == null || !result.Results.Any())
			{
				return null; 
			}

			var projects = result.Results;
			return await GenerateSpreadsheet(projects);
		}

		private async Task<Stream> GenerateSpreadsheet(IEnumerable<ExportedTransferProjectModel> projects)
		{
			string[] headers = [
				"Academy", "URN", "Academy Type", "Incoming Trust", "Incoming Trust UKPRN","Outgoing Trust", "Outgoing Trust UKPRN", "Local Authority", "Region",
				"Advisory Board Date", "Decision Date", "Status", "Decision maker's name", "Decision maker's role", "Assigned To", "Reason for transfer", "Type of transfer",
				"Proposed academy transfer date", "PFI (Private Finance Initiative)"
			];

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
				var advisoryBoardDecision = await advisoryBoardDecisionQueryService.GetByProjectId(project.Id, true);
				worksheet.Cell(row, 1).Value = project.SchoolName;
				worksheet.Cell(row, 2).Value = project.Urn;
				worksheet.Cell(row, 3).Value = project.SchoolType;
				worksheet.Cell(row, 4).Value = project.IncomingTrustName;
				worksheet.Cell(row, 5).Value = project.IncomingTrustUkprn;
				worksheet.Cell(row, 6).Value = project.OutgoingTrustName;
				worksheet.Cell(row, 7).Value = project.OutgoingTrustUKPRN;
				worksheet.Cell(row, 8).Value = project.LocalAuthority;
				worksheet.Cell(row, 9).Value = project.Region;
				worksheet.Cell(row, 10).Value = project.AdvisoryBoardDate;
				worksheet.Cell(row, 11).Value = project.DecisionDate;
				worksheet.Cell(row, 12).Value = project.Status;
				worksheet.Cell(row, 13).Value = advisoryBoardDecision?.DecisionMakerName;
				worksheet.Cell(row, 14).Value = advisoryBoardDecision?.DecisionMadeBy.ToString();
				worksheet.Cell(row, 15).Value = project.AssignedUserFullName;
				worksheet.Cell(row, 16).Value = project.TransferReason;
				worksheet.Cell(row, 17).Value = project.TransferType;
				worksheet.Cell(row, 18).Value = project.ProposedAcademyTransferDate;
				worksheet.Cell(row, 19).Value = project.PFI;
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
