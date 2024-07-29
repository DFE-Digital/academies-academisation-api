using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class ProjectGroupQueryServiceTests
	{
		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private Mock<ILogger<ProjectGroupQueryService>> _mockLogger;
		private ProjectGroupQueryService _projectGroupQueryService;
		private CancellationToken _cancellationToken;
		private readonly Fixture _fixture;

		public ProjectGroupQueryServiceTests()
		{
			_mockProjectGroupRepository = new Mock<IProjectGroupRepository>();
			_mockConversionProjectRepository = new Mock<IConversionProjectRepository>();
			_mockLogger = new Mock<ILogger<ProjectGroupQueryService>>();
			_fixture = new();
			_projectGroupQueryService = new ProjectGroupQueryService(
				_mockProjectGroupRepository.Object,
				_mockConversionProjectRepository.Object,
				_mockLogger.Object
			);
			_cancellationToken = CancellationToken.None;
		}

		[Fact]
		public void GetProjectGroupsAsync_WithReferenceNumber_ShouldReturnResult()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 1, "GRP_0987372", null, null);
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			_mockConversionProjectRepository.Setup(x => x.SearchProjectsV2(null, null, null, null, null, null, searchModel.Page, searchModel.Count)).ReturnsAsync(([], 0));
			_mockProjectGroupRepository.Setup(x => x.SearchProjectGroups(searchModel.Page, searchModel.Count, searchModel.ReferenceNumber, new List<string?>(), _cancellationToken))
				.ReturnsAsync(([expectedProjectGroup], searchModel.Count));
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([expectedProject]);

			// Action
			var result = _projectGroupQueryService.GetProjectGroupsAsync(searchModel, _cancellationToken);

			//Assert
			var pageResult = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(result.Result);
			Assert.Equal(pageResult.Paging.RecordCount, searchModel.Count);
			Assert.Equal(pageResult.Paging.Page, searchModel.Page);
			foreach (var data in pageResult.Data)
			{
				Assert.Equal(data.Urn, expectedProjectGroup.ReferenceNumber);
				Assert.Equal(data.TrustReferenceNumber, expectedProjectGroup.TrustReference);
			}
			_mockConversionProjectRepository.Verify(x => x.SearchProjectsV2(null, null, null, null, null, null, searchModel.Page, searchModel.Count), Times.Once());
			_mockProjectGroupRepository.Verify(x => x.SearchProjectGroups(searchModel.Page, searchModel.Count, searchModel.ReferenceNumber, new List<string>(), _cancellationToken), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken), Times.Once());

		}

		[Fact]
		public void GetProjectGroupsAsync_WithTrustReference_ShouldReturnResult()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 1, null, "83639876", null);
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			_mockConversionProjectRepository.Setup(x => x.SearchProjectsV2(null, null, null, null, null, null, searchModel.Page, searchModel.Count)).ReturnsAsync(([], 0));
			_mockProjectGroupRepository.Setup(x => x.SearchProjectGroups(searchModel.Page, searchModel.Count, searchModel.ReferenceNumber, new List<string?>() { searchModel.TrustReference}, _cancellationToken))
				.ReturnsAsync(([expectedProjectGroup], searchModel.Count));
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([expectedProject]);

			// Action
			var result = _projectGroupQueryService.GetProjectGroupsAsync(searchModel, _cancellationToken);

			//Assert
			var pageResult = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(result.Result);
			Assert.Equal(pageResult.Paging.RecordCount, searchModel.Count);
			Assert.Equal(pageResult.Paging.Page, searchModel.Page);
			foreach (var data in pageResult.Data)
			{
				Assert.Equal(data.Urn, expectedProjectGroup.ReferenceNumber);
				Assert.Equal(data.TrustReferenceNumber, expectedProjectGroup.TrustReference);
			}
			_mockConversionProjectRepository.Verify(x => x.SearchProjectsV2(null, null, null, null, null, null, searchModel.Page, searchModel.Count), Times.Once());
			_mockProjectGroupRepository.Verify(x => x.SearchProjectGroups(searchModel.Page, searchModel.Count, searchModel.ReferenceNumber, new List<string?>() { searchModel.TrustReference }, _cancellationToken), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken), Times.Once());

		}

		[Fact]
		public void GetProjectGroupsAsync_WithTitle_ShouldReturnResult()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 1, null, null, "title");
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			_mockConversionProjectRepository.Setup(x => x.SearchProjectsV2(null,searchModel.Title, null, null, null, null, searchModel.Page, searchModel.Count)).ReturnsAsync(([expectedProject], 1));
			_mockProjectGroupRepository.Setup(x => x.SearchProjectGroups(searchModel.Page, searchModel.Count, searchModel.ReferenceNumber, new List<string?>() { expectedProject.Details.TrustReferenceNumber }, _cancellationToken))
				.ReturnsAsync(([expectedProjectGroup], searchModel.Count));
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([expectedProject]);

			// Action
			var result = _projectGroupQueryService.GetProjectGroupsAsync(searchModel, _cancellationToken);

			//Assert
			var pageResult = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(result.Result);
			Assert.Equal(pageResult.Paging.RecordCount, searchModel.Count);
			Assert.Equal(pageResult.Paging.Page, searchModel.Page);
			foreach (var data in pageResult.Data)
			{
				Assert.Equal(data.Urn, expectedProjectGroup.ReferenceNumber);
				Assert.Equal(data.TrustReferenceNumber, expectedProjectGroup.TrustReference);
			}
			_mockConversionProjectRepository.Verify(x => x.SearchProjectsV2(null, searchModel.Title, null, null, null, null, searchModel.Page, searchModel.Count), Times.Once());
			_mockProjectGroupRepository.Verify(x => x.SearchProjectGroups(searchModel.Page, searchModel.Count, searchModel.ReferenceNumber, new List<string?>() { expectedProject.Details.TrustReferenceNumber }, _cancellationToken), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken), Times.Once());

		}
	}
}
