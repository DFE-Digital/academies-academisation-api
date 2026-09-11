
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries.SignificantChange
{
	public class GetSignificantChangeFilterParametersQueryHandlerTests
	{
		private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock = new();
		private readonly GetSignificantChangeFilterParametersQueryHandler _handler;

		public GetSignificantChangeFilterParametersQueryHandlerTests()
		{
			_handler = new GetSignificantChangeFilterParametersQueryHandler(_repositoryMock.Object);
		}

		[Fact]
		public async Task Handle_ReturnsFilterParametersFromRepository()
		{
			// Arrange
			var cancellationToken = CancellationToken.None;
			var filterParameters = new SignificantChangeFilterParameters
			{
				Statuses = [new FilterValueDisplay("PreDecision", "Pre decision")],
				AssignedUsers = [new FilterValueDisplay("Assigned User", "Assigned User")],
				Tiers = [new FilterValueDisplay("1", "Tier 1"), new FilterValueDisplay("2", "Tier 2")],
				Routes = [new FilterValueDisplay("Change of age range", "Change of age range")]
			};

			_repositoryMock
				.Setup(x => x.GetFilterParameters(cancellationToken))
				.ReturnsAsync(filterParameters);

			// Act
			var result = await _handler.Handle(new GetSignificantChangeFilterParametersQuery(), cancellationToken);

			// Assert
			result.Should().BeSameAs(filterParameters);
		}

		[Fact]
		public async Task Handle_PassesCancellationToken_ToRepository()
		{
			// Arrange
			using var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;

			_repositoryMock
				.Setup(x => x.GetFilterParameters(It.IsAny<CancellationToken>()))
				.ReturnsAsync(new SignificantChangeFilterParameters());

			// Act
			await _handler.Handle(new GetSignificantChangeFilterParametersQuery(), cancellationToken);

			// Assert
			_repositoryMock.Verify(
				x => x.GetFilterParameters(It.Is<CancellationToken>(token => token == cancellationToken)),
				Times.Once);
		}
	}
}