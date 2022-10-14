using Dfe.Academies.Academisation.Service.Factories;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Factories;

public class PagingResponseFactoryTests
{
	[Fact]
	public void Create__WhenThereAreMoreRecordToView__ReturnsNextPageLink()
	{
		// Arrange
		(string path, int page, int requestedCount, int recordCount) = ("test-path", 1, 2, 4);
		Dictionary<string, object?> routeValues = new() { { "states", "active,complete" }, { "urn", 1234 } };

		// Act
		var result = PagingResponseFactory.Create(path, page, requestedCount, recordCount, routeValues);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result.Page),
			() => Assert.Equal(recordCount, result.RecordCount),
			() => Assert.Equal(
				$"{path}?page={page + 1}&count={requestedCount}&states={routeValues["states"]}&urn={routeValues["urn"]}",
				result.NextPageUrl)
		);
	}

	[Fact]
	public void Create__WhenRouteValuesAreEmpty__RouteValuesAreNotPresentInNextPageLink()
	{
		// Arrange
		(string path, int page, int requestedCount, int recordCount) = ("test-path", 1, 2, 4);
		Dictionary<string, object?> routeValues = new();

		// Act
		var result = PagingResponseFactory.Create(path, page, requestedCount, recordCount, routeValues);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result.Page),
			() => Assert.Equal(recordCount, result.RecordCount),
			() => Assert.Equal($"{path}?page={page + 1}&count={requestedCount}", result.NextPageUrl)
		);
	}

	[Fact]
	public void Create__WhenThereAreNoRecords__ReturnsNoNextPageLink()
	{
		// Arrange
		(string path, int page, int requestedCount, int recordCount) = ("test-path", 1, 2, 2);
		Dictionary<string, object?> routeValues = new() { { "states", "active,complete" }, { "urn", 1234 } };

		// Act
		var result = PagingResponseFactory.Create(path, page, requestedCount, recordCount, routeValues);

		// Assert
		Assert.Multiple(
			() => Assert.Equal(page, result.Page),
			() => Assert.Equal(recordCount, result.RecordCount),
			() => Assert.Null(result.NextPageUrl)
		);
	}
}
