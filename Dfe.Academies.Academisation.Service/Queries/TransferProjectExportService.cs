using ClosedXML.Excel;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class TransferProjectExportService : ITransferProjectExportService
	{
		private readonly ITransferProjectQueryService _transferProjectQueryService;

		public TransferProjectExportService(ITransferProjectQueryService transferProjectQueryService)
		{
			_transferProjectQueryService = transferProjectQueryService;
		}

		public async Task<Stream?> ExportTransferProjectsToSpreadsheet(TransferProjectSearchModel searchModel)
		{
			var result = await _transferProjectQueryService.GetExportedTransferProjects(searchModel.TitleFilter);

			if (result?.Results == null || !result.Results.Any())
			{
				return null;
			}

			var projects = result.Results;
			return await GenerateSpreadsheet(projects);
		}

		private async Task<Stream> GenerateSpreadsheet(IEnumerable<ExportedTransferProjectModel> projects)
		{
			var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Projects");
			worksheet.Cell(1, 1).Value = "School";
			worksheet.Cell(1, 1).Style.Font.Bold = true;
			worksheet.Cell(1, 2).Value = "URN";
			worksheet.Cell(1, 2).Style.Font.Bold = true;
			worksheet.Cell(1, 3).Value = "School type";
			worksheet.Cell(1, 3).Style.Font.Bold = true;
			worksheet.Cell(1, 4).Value = "Incoming Trust";
			worksheet.Cell(1, 4).Style.Font.Bold = true;
			worksheet.Cell(1, 5).Value = "Outgoing Trust";
			worksheet.Cell(1, 5).Style.Font.Bold = true;
			worksheet.Cell(1, 6).Value = "Incoming Trust UKPRN";
			worksheet.Cell(1, 6).Style.Font.Bold = true;
			worksheet.Cell(1, 7).Value = "Local Authority";
			worksheet.Cell(1, 7).Style.Font.Bold = true;
			worksheet.Cell(1, 8).Value = "Region";
			worksheet.Cell(1, 8).Style.Font.Bold = true;
			worksheet.Cell(1, 9).Value = "Advisory Board Date";
			worksheet.Cell(1, 9).Style.Font.Bold = true;
			worksheet.Cell(1, 10).Value = "Decision Date";
			worksheet.Cell(1, 10).Style.Font.Bold = true;
			worksheet.Cell(1, 11).Value = "Status";
			worksheet.Cell(1, 11).Style.Font.Bold = true;
			worksheet.Cell(1, 12).Value = "Assigned To";
			worksheet.Cell(1, 12).Style.Font.Bold = true;
			worksheet.Cell(1, 13).Value = "Reason for transfer";
			worksheet.Cell(1, 13).Style.Font.Bold = true;
			worksheet.Cell(1, 14).Value = "Type of transfer";
			worksheet.Cell(1, 14).Style.Font.Bold = true;
			worksheet.Cell(1, 15).Value = "Proposed academy transfer date";
			worksheet.Cell(1, 15).Style.Font.Bold = true;

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

			return stream;
		}
	}
}
