using AutoFixture;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
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
		_query.Setup(m => m.SearchProjects(It.IsAny<List<string>?>(), It.IsAny<int>(), 
				It.IsAny<int>(), It.IsAny<int?>()))
			.ReturnsAsync(expectedProjects);
		
		// Act
		var result = await _subject.GetProjects("complete,active", 1, 1, 1234);
	
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
		_query.Setup(m => m.SearchProjects(It.IsAny<List<string>?>(), It.IsAny<int>(), 
				It.IsAny<int>(), It.IsAny<int?>()))
			.ReturnsAsync(expectedProjects);
		(string states, int page, int count, int? urn) = ("active,complete", 1, 1, 123);
		
		// Act
		var result = await _subject.GetProjects(states, page, count, urn);
	
		// Assert
		var statusList = states.Split(",").ToList();
		_query.Verify(m => m.SearchProjects(statusList, page, count, urn),
			Times.Once);
	}
	
	[Fact]
	public async Task GetProjects__PerformSearch__ReturnsPagingResult()
	{
		// Arrange
		var expectedProjects = _fixture.Create<List<Project>>();
		_query.Setup(m => m.SearchProjects(It.IsAny<List<string>?>(), It.IsAny<int>(), 
				It.IsAny<int>(), It.IsAny<int?>()))
			.ReturnsAsync(expectedProjects);
		(string states, int page, int count, int? urn) = ("active,complete", 1, expectedProjects.Count, 123);
		
		// Act
		var result = await _subject.GetProjects(states, page, count, urn);
	
		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result!.Paging.Page),
			() => Assert.Equal(expectedProjects.Count, result!.Paging.RecordCount),
			() => Assert.Equal($"legacy/projects?page={page + 1}&count={count}&states={states}&urn={urn}", 
				result!.Paging.NextPageUrl)
		);
	}
}
