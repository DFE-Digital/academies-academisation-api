using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class TransferProjectExportServiceTests
	{
		private readonly Mock<ITransferProjectQueryService> _mockTransferProjectQueryService;
		private readonly TransferProjectExportService _service;

		public TransferProjectExportServiceTests()
		{
			_mockTransferProjectQueryService = new Mock<ITransferProjectQueryService>();
			_service = new TransferProjectExportService(_mockTransferProjectQueryService.Object);
		}

		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsStream_WhenProjectsFound()
		{
			// Arrange
			var sampleProjects = new List<ExportedTransferProjectModel>
			{
				new() { SchoolName = "Sample School 1", Status = "Active" },
				new() { SchoolName = "Sample School 2", Status = "Pending" }
			};

			var apiResponse = new PagedResultResponse<ExportedTransferProjectModel>(sampleProjects, 2);

			_mockTransferProjectQueryService.Setup(s => s.GetExportedTransferProjects(null))
				.ReturnsAsync(apiResponse);

			var searchModel = new TransferProjectSearchModel(null);

			// Act
			var result = await _service.ExportTransferProjectsToSpreadsheet(searchModel);

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsNull_WhenNoProjectsFound()
		{
			// Arrange
			var sampleProjects = new List<ExportedTransferProjectModel>
			{
				new () { SchoolName = "Sample School 1", Status = "Active" },
				new () { SchoolName = "Sample School 2", Status = "Pending" }
			};
			var apiResponse = new PagedResultResponse<ExportedTransferProjectModel>(sampleProjects, 2);

			_mockTransferProjectQueryService.Setup(s => s.GetExportedTransferProjects(""))
				.ReturnsAsync(apiResponse);

			var searchModel = new TransferProjectSearchModel(null);

			// Act
			var result = await _service.ExportTransferProjectsToSpreadsheet(searchModel);

			// Assert
			Assert.Null(result);
		}
	}
}
