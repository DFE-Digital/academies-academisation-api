using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup.QueryService;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class ProjectGroupQueryServiceTests
	{
		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private ProjectGroupQueryService _projectGroupQueryService;
		private CancellationToken _cancellationToken;
		private readonly Fixture _fixture;

		public ProjectGroupQueryServiceTests()
		{
			_mockProjectGroupRepository = new Mock<IProjectGroupRepository>();
			_mockConversionProjectRepository = new Mock<IConversionProjectRepository>();
			_fixture = new();
			_projectGroupQueryService = new ProjectGroupQueryService(
				_mockProjectGroupRepository.Object,
				_mockConversionProjectRepository.Object
			);
			_cancellationToken = CancellationToken.None;
		}

		[Fact]
		public async Task GetProjectGroupsAsync_WithReferenceNumber_ShouldReturnResultAsync()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 1, "GRP_0987372", null, null);
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			var emptyList = new List<string>();
			_mockProjectGroupRepository.Setup(x => x.SearchProjectGroups(searchModel.ReferenceNumber!, _cancellationToken))
				.ReturnsAsync([expectedProjectGroup]);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupIdsAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([expectedProject]);

			// Action
			var result = await _projectGroupQueryService.GetProjectGroupsAsync(emptyList, searchModel.ReferenceNumber, emptyList, emptyList, emptyList, emptyList, 1, 1, _cancellationToken);

			//Assert
			var pageResult = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(result);
			Assert.Equal(pageResult.Paging.RecordCount, searchModel.Count);
			Assert.Equal(pageResult.Paging.Page, searchModel.Page);
			foreach (var data in pageResult.Data)
			{
				Assert.Equal(data.ReferenceNumber, expectedProjectGroup.ReferenceNumber);
				Assert.Equal(data.TrustReferenceNumber, expectedProjectGroup.TrustReference);
			}
			_mockProjectGroupRepository.Verify(x => x.SearchProjectGroups(searchModel.ReferenceNumber!, _cancellationToken), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdsAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken), Times.Once());

		}

		[Fact]
		public async Task GetProjectGroupsAsync_WithTrustReference_ShouldReturnResult()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 1, null, "83639876", null);
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			_mockProjectGroupRepository.Setup(x => x.SearchProjectGroups(searchModel.TrustReference!, _cancellationToken))
				.ReturnsAsync([expectedProjectGroup]);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupIdsAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([expectedProject]);
			var emptyList = new List<string>();
			// Action
			var result = await _projectGroupQueryService.GetProjectGroupsAsync(emptyList, searchModel.TrustReference, emptyList, emptyList, emptyList, emptyList, 1, 1, _cancellationToken);

			//Assert
			var pageResult = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(result);
			Assert.Equal(pageResult.Paging.RecordCount, searchModel.Count);
			Assert.Equal(pageResult.Paging.Page, searchModel.Page);
			foreach (var data in pageResult.Data)
			{
				Assert.Equal(data.ReferenceNumber, expectedProjectGroup.ReferenceNumber);
				Assert.Equal(data.TrustReferenceNumber, expectedProjectGroup.TrustReference);
			}
			_mockProjectGroupRepository.Verify(x => x.SearchProjectGroups(searchModel.TrustReference!, _cancellationToken), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdsAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken), Times.Once());

		}

		[Fact]
		public async Task GetProjectGroupsAsync_WithTitle_ShouldReturnResult()
		{
			// Arrange
			var searchModel = new ProjectGroupSearchModel(1, 1, null, null, "title");
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			_mockProjectGroupRepository.Setup(x => x.SearchProjectGroups(searchModel.Title!, _cancellationToken))
				.ReturnsAsync([expectedProjectGroup]);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupIdsAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([expectedProject]);
			var emptyList = new List<string>();
			// Action
			var result = await _projectGroupQueryService.GetProjectGroupsAsync(emptyList, searchModel.Title, emptyList, emptyList, emptyList, emptyList, 1, 1, _cancellationToken);

			//Assert
			var pageResult = Assert.IsType<PagedDataResponse<ProjectGroupResponseModel>>(result);
			Assert.Equal(pageResult.Paging.RecordCount, searchModel.Count);
			Assert.Equal(pageResult.Paging.Page, searchModel.Page);
			foreach (var data in pageResult.Data)
			{
				Assert.Equal(data.ReferenceNumber, expectedProjectGroup.ReferenceNumber);
				Assert.Equal(data.TrustReferenceNumber, expectedProjectGroup.TrustReference);
			}
			_mockProjectGroupRepository.Verify(x => x.SearchProjectGroups(searchModel.Title!, _cancellationToken), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdsAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken), Times.Once());
		}

		[Fact]
		public async Task GetProjectGroupByIdAsync_WithId_ShouldReturnResult()
		{
			// Arrange 
			var expectedProjectGroup = _fixture.Create<ProjectGroup>();
			var expectedProject = _fixture.Create<Domain.ProjectAggregate.Project>();
			expectedProject.SetProjectGroupId(expectedProjectGroup.Id);
			_mockProjectGroupRepository.Setup(x => x.GetById(expectedProjectGroup.Id))
				.ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([expectedProject]);

			// Action
			var result = await _projectGroupQueryService.GetProjectGroupByIdAsync(expectedProjectGroup.Id, _cancellationToken);

			//Assert
			var responseModel = Assert.IsType<ProjectGroupResponseModel>(result);
			Assert.Equal(responseModel.TrustName, expectedProjectGroup.TrustName);
			Assert.Equal(responseModel.ReferenceNumber, expectedProjectGroup.ReferenceNumber);
			Assert.Equal(responseModel.Id, expectedProjectGroup.Id);
			foreach (var project in responseModel.Projects)
			{
				Assert.Equal(project.Id, expectedProject.Id);
				Assert.Equal(project.NameOfTrust, expectedProject.Details.NameOfTrust);
			}
			_mockProjectGroupRepository.Verify(x => x.GetById(expectedProjectGroup.Id), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once());
		}
	}
}
