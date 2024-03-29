﻿using ClosedXML.Excel;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academisation.ExtensionMethods;

namespace Dfe.Academies.Academisation.Service.Queries
{
	public class ConversionProjectExportService : IConversionProjectExportService
	{
		private readonly IConversionProjectQueryService _conversionProjectQueryService;
		private readonly IAdvisoryBoardDecisionGetDataByProjectIdQuery _advisoryBoardDecisionGetDataByProjectIdQuery;

		public ConversionProjectExportService(
			IConversionProjectQueryService conversionProjectQueryService,
			IAdvisoryBoardDecisionGetDataByProjectIdQuery advisoryBoardDecisionGetDataByProjectIdQuery
			)
		{
			_conversionProjectQueryService = conversionProjectQueryService;
			_advisoryBoardDecisionGetDataByProjectIdQuery = advisoryBoardDecisionGetDataByProjectIdQuery;
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
			worksheet.Cell(1, 1).Value = "School";
			worksheet.Cell(1, 1).Style.Font.Bold = true;
			worksheet.Cell(1, 2).Value = "URN";
			worksheet.Cell(1, 2).Style.Font.Bold = true;
			worksheet.Cell(1, 3).Value = "School type";
			worksheet.Cell(1, 3).Style.Font.Bold = true;
			worksheet.Cell(1, 4).Value = "Incoming Trust";
			worksheet.Cell(1, 4).Style.Font.Bold = true;
			worksheet.Cell(1, 5).Value = "Incoming Trust Reference Number";
			worksheet.Cell(1, 5).Style.Font.Bold = true;
			worksheet.Cell(1, 6).Value = "Local Authority";
			worksheet.Cell(1, 6).Style.Font.Bold = true;
			worksheet.Cell(1, 7).Value = "Region";
			worksheet.Cell(1, 7).Style.Font.Bold = true;
			worksheet.Cell(1, 8).Value = "Advisory Board Date";
			worksheet.Cell(1, 8).Style.Font.Bold = true;
			worksheet.Cell(1, 9).Value = "Decision Date";
			worksheet.Cell(1, 9).Style.Font.Bold = true;
			worksheet.Cell(1, 10).Value = "Status";
			worksheet.Cell(1, 10).Style.Font.Bold = true;
			worksheet.Cell(1, 11).Value = "Assigned To";
			worksheet.Cell(1, 11).Style.Font.Bold = true;
			worksheet.Cell(1, 12).Value = "Academy Type and Route";
			worksheet.Cell(1, 12).Style.Font.Bold = true;
			worksheet.Cell(1, 13).Value = "Part of PFI scheme";
			worksheet.Cell(1, 13).Style.Font.Bold = true;


			worksheet.Cell(1, 14).Value = "Date AO issued";
			worksheet.Cell(1, 14).Style.Font.Bold = true;
			worksheet.Cell(1, 15).Value = "Date dAO issued";
			worksheet.Cell(1, 15).Style.Font.Bold = true;
			int row = 2;
			foreach (var project in projects)
			{
				worksheet.Cell(row, 1).Value = project.SchoolName;
				worksheet.Cell(row, 2).Value = project.Urn;
				worksheet.Cell(row, 3).Value = project.SchoolType;
				worksheet.Cell(row, 4).Value = project.NameOfTrust;
				worksheet.Cell(row, 5).Value = project.TrustReferenceNumber;
				worksheet.Cell(row, 6).Value = project.LocalAuthority;
				worksheet.Cell(row, 7).Value = project.Region;
				worksheet.Cell(row, 8).Value = project.HeadTeacherBoardDate;
				var advisoryBoardDecision = await _advisoryBoardDecisionGetDataByProjectIdQuery.Execute(project.Id);
				worksheet.Cell(row, 9).Value = advisoryBoardDecision?.AdvisoryBoardDecisionDetails.AdvisoryBoardDecisionDate;
				worksheet.Cell(row, 10).Value = project.ProjectStatus;
				worksheet.Cell(row, 11).Value = project.AssignedUser?.FullName;
				worksheet.Cell(row, 12).Value = project.AcademyTypeAndRoute;
				worksheet.Cell(row, 13).Value = project.PartOfPfiScheme;
				if (project.AcademyTypeAndRoute?.ToLower().Equals("sponsored") ?? false)
				{
					worksheet.Cell(row, 15).Value = project.DaoPackSentDate.ToDateString();
				}
				else
				{
					worksheet.Cell(row, 14).Value = advisoryBoardDecision?.AdvisoryBoardDecisionDetails.AcademyOrderDate.ToDateString();
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
