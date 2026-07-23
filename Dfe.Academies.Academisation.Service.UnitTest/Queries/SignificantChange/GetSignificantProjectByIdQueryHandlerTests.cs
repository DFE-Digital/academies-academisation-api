using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries.SignificantChange
{
	public class GetSignificantProjectByIdQueryHandlerTests
	{
		private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
		private readonly GetSignificantProjectByIdQueryHandler _handler;

		public GetSignificantProjectByIdQueryHandlerTests()
		{
			_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
			_handler = new GetSignificantProjectByIdQueryHandler(_repositoryMock.Object);
		}

		[Fact]
		public async Task Handle_ProjectExists_ReturnsMappedResponse()
		{
			var query = new GetSignificantProjectByIdQuery(10);
			var cancellationToken = CancellationToken.None;
			var assignedUserId = Guid.NewGuid();

			var project = new SignificantChangeProject(
				SignificantChangeStatus.InProgress,
				urn: 123456,
				tier: 2,
				trustName: "Trust A",
				trustUkprn: "10000001",
				typeOfSignificantChange: "Change of age range",
				schoolName: "School A")
			{
				Id = query.Id
			};

			project.AssignUser(assignedUserId, "assigned.user@test.local", "Assigned User");

			_repositoryMock
				.Setup(x => x.GetSignificantProjectById(query.Id, cancellationToken))
				.ReturnsAsync(project);

			var result = await _handler.Handle(query, cancellationToken);

			result.Should().NotBeNull();
			result!.Id.Should().Be(10);
			result.Urn.Should().Be(123456);
			result.SchoolName.Should().Be("School A");
			result.Tier.Should().Be(2);
			result.TrustName.Should().Be("Trust A");
			result.TrustUkprn.Should().Be("10000001");
			result.AssignedUser.Should().BeEquivalentTo(new User(assignedUserId, "Assigned User", "assigned.user@test.local"));
			result.TypeOfSignificantChange.Should().Be("Change of age range");
			result.Status.Should().Be(nameof(SignificantChangeStatus.InProgress));
		}

		[Fact]
		public async Task Handle_ProjectDoesNotExist_ReturnsNull()
		{
			var query = new GetSignificantProjectByIdQuery(999);
			using var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;

			_repositoryMock
				.Setup(x => x.GetSignificantProjectById(query.Id, cancellationToken))
				.ReturnsAsync((SignificantChangeProject?)null);

			var result = await _handler.Handle(query, cancellationToken);

			result.Should().BeNull();
		}
	}
}