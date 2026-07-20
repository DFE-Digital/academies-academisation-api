using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries.SignificantChange
{
	public class GetSignificantProjectsQueryHandlerTests
	{
		private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
		private readonly GetSignificantProjectsQueryHandler _handler;

		public GetSignificantProjectsQueryHandlerTests()
		{
			_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
			var dateTimeProviderMock = new Mock<IDateTimeProvider>();
			var loggerMock = new Mock<ILogger<GetSignificantProjectsQueryHandler>>();

			_handler = new GetSignificantProjectsQueryHandler(
				_repositoryMock.Object);
		}

		[Fact]
		public async Task Handle_ValidQuery_ReturnsMappedPagedResponse()
		{
			// Arrange
			var query = new GetSignificantProjectsQuery(Page: 1, Count: 2);
			var cancellationToken = CancellationToken.None;
			var projects = new List<SignificantChangeProject>
			{
				CreateProject(id: 10, urn: 123456, tier: 2, trustName: "Trust A", trustUkprn: "10000001", route: "Change of age range"),
				CreateProject(id: 11, urn: 654321, tier: 3, trustName: "Trust B", trustUkprn: "10000002", route: "Change of gender composition")
			};

			_repositoryMock
				.Setup(x => x.SearchSignificantProjects(query.Page, query.Count, cancellationToken))
				.ReturnsAsync((projects, totalCount: 3));

			// Act
			var result = await _handler.Handle(query, cancellationToken);
			var data = result.Data.ToList();

			// Assert
			result.Paging.Page.Should().Be(1);
			result.Paging.RecordCount.Should().Be(3);
			result.Paging.NextPageUrl.Should().Be("significant-change/significant-change-projects?page=2&count=2");

			data.Should().HaveCount(2);
			data[0].Should().BeEquivalentTo(new SignificantChangeProjectResponse
			{
				Id = 10,
				Urn = 123456,
				Tier = 2,
				TrustName = "Trust A",
				TrustUkprn = "10000001",
				TypeOfSignificantChange = "Change of age range",
				Status = nameof(SignificantChangeStatus.InProgress)
			});
			data[1].Should().BeEquivalentTo(new SignificantChangeProjectResponse
			{
				Id = 11,
				Urn = 654321,
				Tier = 3,
				TrustName = "Trust B",
				TrustUkprn = "10000002",
				TypeOfSignificantChange = "Change of gender composition",
				Status = nameof(SignificantChangeStatus.InProgress)
			});
		}

		[Fact]
		public async Task Handle_LastPage_DoesNotSetNextPageUrl()
		{
			// Arrange
			var query = new GetSignificantProjectsQuery(Page: 2, Count: 2);
			var cancellationToken = CancellationToken.None;
			var projects = new List<SignificantChangeProject>
			{
				CreateProject(id: 22, urn: 222222, tier: 2, trustName: "Trust C", trustUkprn: "10000003", route: "Other")
			};

			_repositoryMock
				.Setup(x => x.SearchSignificantProjects(query.Page, query.Count, cancellationToken))
				.ReturnsAsync((projects, totalCount: 4));

			// Act
			var result = await _handler.Handle(query, cancellationToken);

			// Assert
			result.Paging.Page.Should().Be(2);
			result.Paging.RecordCount.Should().Be(4);
			result.Paging.NextPageUrl.Should().BeNull();
		}

		[Fact]
		public async Task Handle_PassesPageCountAndCancellationToken_ToRepository()
		{
			// Arrange
			var query = new GetSignificantProjectsQuery(Page: 3, Count: 25);
			using var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;

			_repositoryMock
				.Setup(x => x.SearchSignificantProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Enumerable.Empty<SignificantChangeProject>(), 0));

			// Act
			await _handler.Handle(query, cancellationToken);

			// Assert
			_repositoryMock.Verify(
				x => x.SearchSignificantProjects(
					It.Is<int>(page => page == 3),
					It.Is<int>(count => count == 25),
					It.Is<CancellationToken>(token => token == cancellationToken)),
				Times.Once);
		}

		private static SignificantChangeProject CreateProject(int id, int urn, byte tier, string trustName, string trustUkprn, string route)
		{
			return new SignificantChangeProject(SignificantChangeStatus.InProgress, urn, tier, trustName, trustUkprn, route)
			{
				Id = id,
				CreatedOn = DateTime.UtcNow
			};
		}
	}
}
