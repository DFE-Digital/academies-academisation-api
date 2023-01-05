﻿using AutoFixture;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Fare;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries;

public class LegacyProjectListGetQueryTests
{
	private readonly LegacyProjectListGetQuery _subject;
	private readonly Mock<IProjectListGetDataQuery> _query = new();
	private readonly Fixture _fixture = new();
	
	public LegacyProjectListGetQueryTests()
	{
		_subject = new(_query.Object);
	}
	
	[Fact]
	public async Task GetProjects__PerformSearch__ReturnsProjects()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<IEnumerable<string>?>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<int>(), 
				It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>?>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		List<string> status = new List<string>() { "complete", "active"};
		// Act
		var result = await _subject.GetProjects(status, null, null,  1, 1, 1234, default);
	
		// Assert
		Assert.Multiple(
			() => Assert.Equal(expectedProjects[0].Id, result!.Data.ToList()[0].Id),
			() => Assert.Equal(expectedProjects[1].Id, result!.Data.ToList()[1].Id),
			() => Assert.Equal(expectedProjects[2].Id, result!.Data.ToList()[2].Id));
	}
	
	[Fact]
	public async Task GetProjects__PerformSearch__InvokesSearch()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<IEnumerable<string>?>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<int>(),
				It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>?>()))
				.ReturnsAsync((expectedProjects, expectedProjects.Count));
		(int page, int count, int? urn) = (1, 1, 123);
		List<string>? status = new List<string>() {"complete", "active"};
		// Act
		var result = await _subject.GetProjects(status, null, null, page, count, urn, null);
	
		// Assert
		_query.Verify(m => m.SearchProjects(status, null, null, page, count, urn, null),
			Times.Once);
	}
	
	[Fact]
	public async Task GetProjects__PerformSearch__ReturnsPagingResult()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<IEnumerable<string>?>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<int>(),
				It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<IEnumerable<string>?>()))
			.ReturnsAsync((expectedProjects, expectedProjects.Count));
		(int page, int count, int? urn) = (1, 1, 123);
		List<string> status = new List<string>() { "complete", "active" };
		// Act
		var result = await _subject.GetProjects(status, null, null, page, count, urn, null);
	
		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result!.Paging.Page),
			() => Assert.Equal(expectedProjects.Count, result!.Paging.RecordCount)
		);
	}
}
