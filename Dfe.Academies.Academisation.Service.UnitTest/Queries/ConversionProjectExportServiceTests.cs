using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class ConversionProjectExportServiceTests
	{
		private readonly Mock<IConversionProjectQueryService> _mockConversionProjectQueryService;
		private readonly Mock<IAdvisoryBoardDecisionGetDataByProjectIdQuery> _mockAdvisoryBoardDecisionQuery;
		private readonly ConversionProjectExportService _service;

		public ConversionProjectExportServiceTests()
		{
			_mockConversionProjectQueryService = new Mock<IConversionProjectQueryService>();
			_mockAdvisoryBoardDecisionQuery = new Mock<IAdvisoryBoardDecisionGetDataByProjectIdQuery>();
			_service = new ConversionProjectExportService(_mockConversionProjectQueryService.Object, _mockAdvisoryBoardDecisionQuery.Object);
		}
		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsStream_WhenProjectsFound()
		{
			// Arrange
			var sampleProjects = new List<ConversionProjectServiceModel>
			{
				new ConversionProjectServiceModel(1, 100) { SchoolName = "Sample School 1", ProjectStatus = "Active" },
				new ConversionProjectServiceModel(2, 200) { SchoolName = "Sample School 2", ProjectStatus = "Pending" }
			};

			var legacyApiResponse = new LegacyApiResponse<ConversionProjectServiceModel>(sampleProjects, new PagingResponse { Page = 1, RecordCount = 2 });

			_mockConversionProjectQueryService.Setup(s => s.GetProjectsV2(
					null,
					null,
					null,
					1,
					int.MaxValue,
					null,
					null,
					null))
				.ReturnsAsync(legacyApiResponse);

			var searchModel = new ConversionProjectSearchModel(
				1,
				10,
				null,
				null,
				null, 
				null, 
				null, 
				null);

			// Act
			var result = await _service.ExportProjectsToSpreadsheet(searchModel);

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public async Task ExportProjectsToSpreadsheet_ReturnsNull_WhenNoProjectsFound()
		{
			// Arrange		
			var sampleProjects = new List<ConversionProjectServiceModel>
			{
				new ConversionProjectServiceModel(1, 100) { SchoolName = "Sample School 1", ProjectStatus = "Active" },
				new ConversionProjectServiceModel(2, 200) { SchoolName = "Sample School 2", ProjectStatus = "Pending" }
			};
			var legacyApiResponse = new LegacyApiResponse<ConversionProjectServiceModel>(sampleProjects, new PagingResponse { Page = 1, RecordCount = 2 });

			_mockConversionProjectQueryService.Setup(s => s.GetProjectsV2(
					null,
					"TitleThatDoesntExist",
					null,
					1,
					int.MaxValue,
					null,
					null,
					null))
				.ReturnsAsync(legacyApiResponse);

			var searchModel = new ConversionProjectSearchModel(
				1,
				10,
				null,
				null,
				null,
				null,
				null,
				null);

			// Act
			var result = await _service.ExportProjectsToSpreadsheet(searchModel);

			// Assert
			Assert.Null(result);
		}



	}

}
