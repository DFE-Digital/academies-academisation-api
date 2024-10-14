using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries;

public class LegacyProjectListGetQueryTests
{
	private readonly ConversionProjectQueryService _subject;
	private readonly Mock<IConversionProjectRepository> _query = new();
	private readonly Mock<IFormAMatProjectRepository> _formAMatQuery = new();
	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockAdvisoryBoardDecisionRepository = new();

	public LegacyProjectListGetQueryTests()
	{
		_fixture.Customize(new AutoMoqCustomization());
		_subject = new(_query.Object, _formAMatQuery.Object, _mockAdvisoryBoardDecisionRepository.Object);
	}

	[Fact]
	public async Task GetProjects__PerformSearch__ReturnsProjects()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<IEnumerable<string>?>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<int>(),
				It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<IEnumerable<string>?>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjects(status, null, null, 1, 1, 1234, default, null);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(expectedProjects[0].Id, result!.Data.ToList()[0].Id),
			() => Assert.Equal(expectedProjects[1].Id, result!.Data.ToList()[1].Id),
			() => Assert.Equal(expectedProjects[2].Id, result!.Data.ToList()[2].Id));
	}

	[Fact]
	public async Task GetProjectsV2__PerformSearch__ReturnsProjects()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjectsV2(It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<string>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<int>(),
									   It.IsAny<int>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjectsV2(status, null, null, 1, 1, null, null, null);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(expectedProjects[0].Id, result!.Data.ToList()[0].Id),
			() => Assert.Equal(expectedProjects[1].Id, result!.Data.ToList()[1].Id),
			() => Assert.Equal(expectedProjects[2].Id, result!.Data.ToList()[2].Id));
	}

	[Fact]
	public async Task GetMatProjects__PerformSearch__ReturnsProjects()
	{
		// Arrange
		var formAMatProjectId = 1010;

		var expectedProjects = _fixture.Create<List<IProject>>();

		foreach (var projectMock in expectedProjects)
		{
			Mock.Get(projectMock).Setup(x => x.FormAMatProjectId).Returns(formAMatProjectId);
		}

		var expectedFormAMatProject = _fixture.Create<IFormAMatProject>();
		Mock.Get(expectedFormAMatProject).Setup(x => x.Id).Returns(formAMatProjectId);
		Mock.Get(expectedFormAMatProject).Setup(x => x.AssignedUser).Returns(new Domain.Core.ProjectAggregate.User(Guid.NewGuid(), "test test", "test@test.com"));

		_query.Setup(m => m.SearchFormAMatProjects(It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<string>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));

		_formAMatQuery.Setup(x => x.GetByIds(It.Is<IEnumerable<int?>>(id => id.Contains(formAMatProjectId)), It.IsAny<CancellationToken>())).ReturnsAsync(new List<IFormAMatProject>() { expectedFormAMatProject });

		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetFormAMatProjects(status, null, null, 1, 1, default, null, null, null);

		// Assert
		result.Should().BeOfType<PagedDataResponse<FormAMatProjectServiceModel>>();
		result.Data.ToList()[0].ProposedTrustName.Should().Be(expectedFormAMatProject.ProposedTrustName);
		result.Data.ToList()[0].ApplicationReference.Should().Be(expectedFormAMatProject.ApplicationReference);

		Assert.Multiple(
			() => Assert.Equal(expectedProjects[0].Id, result!.Data.ToList()[0].projects[0].Id),
			() => Assert.Equal(expectedProjects[1].Id, result!.Data.ToList()[0].projects[1].Id),
			() => Assert.Equal(expectedProjects[2].Id, result!.Data.ToList()[0].projects[2].Id));
	}

	[Fact]
	public async Task GetMatProjects__forSpecificPageAndCount__PerformSearch__ReturnsProjects()
	{
		// Arrange
		var formAMatProjectId1 = 1010;
		var formAMatProjectId2 = 1011;
		var formAMatProjectId3 = 1012;

		var expectedProjects = _fixture.CreateMany<IProject>(10).ToList();

		for (var i = 0; i < expectedProjects.Count; i++)
		{
			var projectMock = expectedProjects[i];
			Mock.Get(projectMock).Setup(x => x.Id).Returns(_fixture.Create<int>());
			if (i < 3)
			{
				Mock.Get(projectMock).Setup(x => x.FormAMatProjectId).Returns(formAMatProjectId1);
				continue;
			}

			if (i < 6)
			{
				Mock.Get(projectMock).Setup(x => x.FormAMatProjectId).Returns(formAMatProjectId2);
				continue;
			}

			if (i < 10)
			{
				Mock.Get(projectMock).Setup(x => x.FormAMatProjectId).Returns(formAMatProjectId3);
			}

		}

		var expectedFormAMatProject1 = _fixture.Create<IFormAMatProject>();
		var expectedFormAMatProject2 = _fixture.Create<IFormAMatProject>();
		var expectedFormAMatProject3 = _fixture.Create<IFormAMatProject>();

		Mock.Get(expectedFormAMatProject1).Setup(x => x.Id).Returns(formAMatProjectId1);
		Mock.Get(expectedFormAMatProject1).Setup(x => x.ProposedTrustName).Returns("New trust 1");
		Mock.Get(expectedFormAMatProject1).Setup(x => x.ApplicationReference).Returns("A2B_123");
		Mock.Get(expectedFormAMatProject1).Setup(x => x.AssignedUser).Returns(new Domain.Core.ProjectAggregate.User(Guid.NewGuid(), "test test", "test@test.com"));
		Mock.Get(expectedFormAMatProject1).Setup(x => x.CreatedOn).Returns(DateTime.Now.AddDays(-1));


		Mock.Get(expectedFormAMatProject2).Setup(x => x.Id).Returns(formAMatProjectId2);
		Mock.Get(expectedFormAMatProject2).Setup(x => x.ProposedTrustName).Returns("New trust 2");
		Mock.Get(expectedFormAMatProject2).Setup(x => x.ApplicationReference).Returns("A2B_456");
		Mock.Get(expectedFormAMatProject2).Setup(x => x.AssignedUser).Returns(new Domain.Core.ProjectAggregate.User(Guid.NewGuid(), "test test", "test@test.com"));
		Mock.Get(expectedFormAMatProject2).Setup(x => x.CreatedOn).Returns(DateTime.Now.AddDays(-2));

		Mock.Get(expectedFormAMatProject3).Setup(x => x.Id).Returns(formAMatProjectId3);
		Mock.Get(expectedFormAMatProject3).Setup(x => x.ProposedTrustName).Returns("New trust 3");
		Mock.Get(expectedFormAMatProject3).Setup(x => x.ApplicationReference).Returns("A2B_789");
		Mock.Get(expectedFormAMatProject3).Setup(x => x.AssignedUser).Returns(new Domain.Core.ProjectAggregate.User(Guid.NewGuid(), "test test", "test@test.com"));
		Mock.Get(expectedFormAMatProject3).Setup(x => x.CreatedOn).Returns(DateTime.Now.AddDays(-3));

		_query.Setup(m => m.SearchFormAMatProjects(It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<string>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>()))
			.ReturnsAsync((expectedProjects, 3));

		_formAMatQuery.Setup(x => x.GetByIds(It.Is<IEnumerable<int?>>(id => id.Contains(formAMatProjectId1)), It.IsAny<CancellationToken>())).ReturnsAsync(new List<IFormAMatProject>() { expectedFormAMatProject1, expectedFormAMatProject2, expectedFormAMatProject3 });

		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetFormAMatProjects(status, null, null, 1, 1, default, null, null, null);
		var result2 = await _subject.GetFormAMatProjects(status, null, null, 2, 1, default, null, null, null);
		var result3 = await _subject.GetFormAMatProjects(status, null, null, 3, 1, default, null, null, null);

		// Assert first result, should be page 1 and have 1 result
		result.Should().BeOfType<PagedDataResponse<FormAMatProjectServiceModel>>();
		result.Data.ToList()[0].ProposedTrustName.Should().Be(expectedFormAMatProject1.ProposedTrustName);
		result.Data.ToList()[0].ApplicationReference.Should().Be(expectedFormAMatProject1.ApplicationReference);

		Assert.Multiple(
			() => Assert.Equal(expectedProjects[0].Id, result!.Data.ToList()[0].projects[0].Id),
			() => Assert.Equal(expectedProjects[1].Id, result!.Data.ToList()[0].projects[1].Id),
			() => Assert.Equal(expectedProjects[2].Id, result!.Data.ToList()[0].projects[2].Id));

		// Assert second result, should be page 2 and have 1 result
		result2.Should().BeOfType<PagedDataResponse<FormAMatProjectServiceModel>>();
		result2.Data.ToList()[0].ProposedTrustName.Should().Be(expectedFormAMatProject2.ProposedTrustName);
		result2.Data.ToList()[0].ApplicationReference.Should().Be(expectedFormAMatProject2.ApplicationReference);

		Assert.Multiple(
			() => Assert.Equal(expectedProjects[3].Id, result2!.Data.ToList()[0].projects[0].Id),
			() => Assert.Equal(expectedProjects[4].Id, result2!.Data.ToList()[0].projects[1].Id),
			() => Assert.Equal(expectedProjects[5].Id, result2!.Data.ToList()[0].projects[2].Id));

		// Assert third result, should be page 3 and have 1 result
		result3.Should().BeOfType<PagedDataResponse<FormAMatProjectServiceModel>>();
		result3.Data.ToList()[0].ProposedTrustName.Should().Be(expectedFormAMatProject3.ProposedTrustName);
		result3.Data.ToList()[0].ApplicationReference.Should().Be(expectedFormAMatProject3.ApplicationReference);

		Assert.Multiple(
			() => Assert.Equal(expectedProjects[6].Id, result3!.Data.ToList()[0].projects[0].Id),
			() => Assert.Equal(expectedProjects[7].Id, result3!.Data.ToList()[0].projects[1].Id),
			() => Assert.Equal(expectedProjects[8].Id, result3!.Data.ToList()[0].projects[2].Id),
			() => Assert.Equal(expectedProjects[9].Id, result3!.Data.ToList()[0].projects[3].Id));
	}

	[Fact]
	public async Task GetProjectsV2__PerformSearch__InvokesSearch()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjectsV2(It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<string>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<int>(),
									   It.IsAny<int>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		(int page, int count) = (1, 2);
		List<string>? status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjectsV2(status, null, null, page, count, null, null, null);

		// Assert
		_query.Verify(m => m.SearchProjectsV2(status, null, null, null, null, null, page, count),
			Times.Once);
	}

	[Fact]
	public async Task GetProjects__PerformSearch__InvokesSearch()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<IEnumerable<string>?>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<int>(),
				It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<IEnumerable<string>?>()))
				.ReturnsAsync((expectedProjects, expectedProjects.Count));
		(int page, int count, int? urn) = (1, 1, 123);
		List<string>? status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjects(status, null, null, page, count, urn, null, null);

		// Assert
		_query.Verify(m => m.SearchProjects(status, null, null, page, count, urn, null, null),
			Times.Once);
	}

	[Fact]
	public async Task GetProjects__PerformSearch__ReturnsPagingResult()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<IEnumerable<string>?>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<int>(),
				It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<IEnumerable<string>?>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		(int page, int count, int? urn) = (1, 1, 123);
		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjects(status, null, null, page, count, urn, null, null);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result!.Paging.Page),
			() => Assert.Equal(expectedProjects.Count, result!.Paging.RecordCount)
		);
	}

	[Fact]
	public async Task GetProjectsV2__PerformSearch__ReturnsPagingResult()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjectsV2(It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<string>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<int>(),
									   It.IsAny<int>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		(int page, int count) = (1, 1);
		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjectsV2(status, null, null, page, count, null, null, null);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result!.Paging.Page),
			() => Assert.Equal(expectedProjects.Count, result!.Paging.RecordCount)
		);
	}
	[Fact]
	public async Task SearchFormAMatProjectsByTermAsync_PerformSearch_ReturnsMatchingProjects()
	{
		// Arrange
		string searchTerm = "test";
		var expectedFormAMatProjects = _fixture.CreateMany<FormAMatProject>(3).ToList();

		_formAMatQuery.Setup(x => x.SearchProjectsByTermAsync(searchTerm, It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedFormAMatProjects);

		// Act
		var result = await _subject.SearchFormAMatProjectsByTermAsync(searchTerm, default);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(expectedFormAMatProjects.Count);
		result.ToList().ForEach(model =>
		{
			var expectedProject = expectedFormAMatProjects.FirstOrDefault(p => p.Id == model.Id);
			expectedProject.Should().NotBeNull();
			model.ProposedTrustName.Should().Be(expectedProject.ProposedTrustName);
			model.ApplicationReference.Should().Be(expectedProject.ApplicationReference);
		});

		_formAMatQuery.Verify(x => x.SearchProjectsByTermAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
	}

}
