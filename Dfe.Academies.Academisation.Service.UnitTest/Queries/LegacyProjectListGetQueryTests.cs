using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Fare;
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

	public LegacyProjectListGetQueryTests()
	{
		_fixture.Customize(new AutoMoqCustomization());
		_subject = new(_query.Object, _formAMatQuery.Object);
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

		_query.Setup(m => m.SearchFormAMatProjects(It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<string>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<IEnumerable<string>?>(),
									   It.IsAny<int>(),
									   It.IsAny<int>()))
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
}
