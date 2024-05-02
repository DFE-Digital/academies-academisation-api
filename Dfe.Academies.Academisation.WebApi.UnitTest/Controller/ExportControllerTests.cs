using System.IO;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller
{
	public class ExportControllerTests
	{
		private readonly Mock<IConversionProjectExportService> _mockConversionProjectExportService;
		private readonly Mock<ITransferProjectExportService> _mockTransferProjectExportService;
		private readonly Mock<IMediator> _mockMediator;
		private readonly ExportController _controller;

		public ExportControllerTests()
		{
			_mockConversionProjectExportService = new Mock<IConversionProjectExportService>();
			_mockTransferProjectExportService = new Mock<ITransferProjectExportService>();
			_mockMediator = new Mock<IMediator>();
			_controller = new ExportController(_mockConversionProjectExportService.Object, _mockTransferProjectExportService.Object);
		}
		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsFileResult_WhenProjectsFound()
		{
			// Arrange
			var mockStream = new MemoryStream();
			var searchModel = new ConversionProjectSearchModel(1, 10, null, null, null, null, null, null);
			_mockConversionProjectExportService.Setup(s => s.ExportProjectsToSpreadsheet(searchModel))
											   .ReturnsAsync(mockStream);

			// Act
			var result = await _controller.ExportProjectsToSpreadsheet(searchModel);

			// Assert
			var fileResult = Assert.IsAssignableFrom<FileResult>(result);
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
			Assert.Equal("exported_projects.xlsx", fileResult.FileDownloadName);
		}

		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsNotFound_WhenNoProjectsFound()
		{
			// Arrange
			_mockConversionProjectExportService.Setup(s => s.ExportProjectsToSpreadsheet(It.IsAny<ConversionProjectSearchModel>()))
											   .ReturnsAsync(null as Stream);
			var searchModel = new ConversionProjectSearchModel(1, 10, null, null, null, null, null, null);

			// Act
			var result = await _controller.ExportProjectsToSpreadsheet(searchModel);

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("No projects found for the specified criteria.", notFoundResult.Value);
		}

		[Fact]
		public async Task ExportTransferProjectsToSpreadsheet_ReturnsFileResult_WhenProjectsFound()
		{
			// Arrange
			var mockStream = new MemoryStream();
			var searchModel = new TransferProjectSearchModel(null);
			_mockTransferProjectExportService.Setup(s => s.ExportTransferProjectsToSpreadsheet(searchModel))
				.ReturnsAsync(mockStream);

			// Act
			var result = await _controller.ExportTransferProjectsToSpreadsheet(searchModel);

			// Assert
			var fileResult = Assert.IsAssignableFrom<FileResult>(result);
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
			Assert.Equal("exported_projects.xlsx", fileResult.FileDownloadName);
		}

		[Fact]
		public async Task ExportTransferProjectsToSpreadsheet_ReturnsNotFound_WhenNoProjectsFound()
		{
			// Arrange
			_mockTransferProjectExportService.Setup(s => s.ExportTransferProjectsToSpreadsheet(It.IsAny<TransferProjectSearchModel>()))
				.ReturnsAsync(null as Stream);
			var searchModel = new TransferProjectSearchModel(null);

			// Act
			var result = await _controller.ExportTransferProjectsToSpreadsheet(searchModel);

			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
			Assert.Equal("No projects found for the specified criteria.", notFoundResult.Value);
		}


	}
}
