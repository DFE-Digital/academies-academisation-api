using AutoMapper;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using FluentAssertions;
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
			var mapperConfiguration = new MapperConfiguration(configuration =>
				configuration.AddProfile<SignificantChangeProjectMappingProfile>());
			mapperConfiguration.AssertConfigurationIsValid();
			var mapper = mapperConfiguration.CreateMapper();

			_handler = new GetSignificantProjectsQueryHandler(
				_repositoryMock.Object,
				mapper);
		}

		[Fact]
		public async Task Handle_ValidQuery_ReturnsMappedPagedResponse()
		{
			var proposedDecisionDate = DateTime.UtcNow.AddDays(10);
			var proposedChangeDate = DateTime.UtcNow.AddDays(20);

			// Arrange
			var query = new GetSignificantProjectsQuery(Page: 1, Count: 2);
			var cancellationToken = CancellationToken.None;
			var assignedUserId = Guid.NewGuid();
			var projects = new List<SignificantChangeProject>
			{
				CreateProject(id: 10, urn: 123456, tier: 2, trustName: "Trust A", trustUkprn: "10000001", route: "Change of age range", schoolName: "School A"),
				CreateProject(id: 11, urn: 654321, tier: 3, trustName: "Trust B", trustUkprn: "10000002", route: "Change of gender composition", schoolName: "School B")
			};

			projects[0].AssignUser(assignedUserId, "assigned.user@test.local", "Assigned User");
			projects[0].SetStakeholderConsultation(false, "Trust has not consulted stakeholders yet");
			projects[0].SetProjectDates(proposedDecisionDate, proposedChangeDate);

			_repositoryMock
				.Setup(x => x.SearchSignificantChangeProjects(query.Page, query.Count, null, null, null, null, null, cancellationToken))
				.ReturnsAsync((projects, totalCount: 3));

			// Act
			var result = await _handler.Handle(query, cancellationToken);
			var data = result.Data.ToList();

			// Assert
			result.Paging.Page.Should().Be(1);
			result.Paging.RecordCount.Should().Be(3);
			result.Paging.NextPageUrl.Should().Be("significant-change/significant-change-projects?page=2&count=2");

			data.Should().HaveCount(2);
			data[0].Id.Should().Be(10);
			data[0].Urn.Should().Be(123456);
			data[0].SchoolName.Should().Be("School A");
			data[0].Tier.Should().Be(2);
			data[0].TrustName.Should().Be("Trust A");
			data[0].TrustUkprn.Should().Be("10000001");
			data[0].AssignedUser.Should().BeEquivalentTo(new User(assignedUserId, "Assigned User", "assigned.user@test.local"));
			data[0].TypeOfSignificantChange.Should().Be("Change of age range");
			data[0].Status.Should().Be(nameof(SignificantChangeStatus.PreDecision));
			data[0].StakeholderConsultation.TrustConsultedStakeholders.Should().BeFalse();
			data[0].StakeholderConsultation.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Trust has not consulted stakeholders yet");
			data[0].StakeholderConsultation.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));
			data[0].ProjectDates.ProposedDecisionDate.Should().Be(proposedDecisionDate);
			data[0].ProjectDates.ProposedChangeDate.Should().Be(proposedChangeDate);
			data[0].ProjectDates.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));

			data[1].Id.Should().Be(11);
			data[1].Urn.Should().Be(654321);
			data[1].SchoolName.Should().Be("School B");
			data[1].Tier.Should().Be(3);
			data[1].TrustName.Should().Be("Trust B");
			data[1].TrustUkprn.Should().Be("10000002");
			data[1].AssignedUser.Should().BeNull();
			data[1].TypeOfSignificantChange.Should().Be("Change of gender composition");
			data[1].Status.Should().Be(nameof(SignificantChangeStatus.PreDecision));
			data[1].StakeholderConsultation.Status.Should().Be(nameof(SignificantChangeTaskStatus.NotStarted));
			data[1].ProjectDates.Status.Should().Be(nameof(SignificantChangeTaskStatus.NotStarted));
		}

		[Fact]
		public async Task Handle_LastPage_DoesNotSetNextPageUrl()
		{
			// Arrange
			var query = new GetSignificantProjectsQuery(Page: 2, Count: 2);
			var cancellationToken = CancellationToken.None;
			var projects = new List<SignificantChangeProject>
			{
				CreateProject(id: 22, urn: 222222, tier: 2, trustName: "Trust C", trustUkprn: "10000003", route: "Other", schoolName: "School C")
			};

			_repositoryMock
				.Setup(x => x.SearchSignificantChangeProjects(query.Page, query.Count, null, null, null, null, null, cancellationToken))
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
				.Setup(x => x.SearchSignificantChangeProjects(It.IsAny<int>(), It.IsAny<int>(), null, null, null, null, null, It.IsAny<CancellationToken>()))
				.ReturnsAsync((Enumerable.Empty<SignificantChangeProject>(), 0));

			// Act
			await _handler.Handle(query, cancellationToken);

			// Assert
			_repositoryMock.Verify(
				x => x.SearchSignificantChangeProjects(
					It.Is<int>(page => page == 3),
					It.Is<int>(count => count == 25),
					null,
					null,
					null,
					null,
					null,
					It.Is<CancellationToken>(token => token == cancellationToken)),
				Times.Once);
		}

		[Fact]
		public async Task Handle_PassesQueryItems_ToRepository()
		{
			// Arrange
			var query = new GetSignificantProjectsQuery(Page: 3, Count: 25, "school", ["InProgress"], ["Ste"], [1], ["a change"]);

			using var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;

			_repositoryMock
				.Setup(x => x.SearchSignificantChangeProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
					It.IsAny<List<string>>(), It.IsAny<List<string>>(), It.IsAny<List<byte>>(),
					It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync((Enumerable.Empty<SignificantChangeProject>(), 0));

			// Act
			await _handler.Handle(query, cancellationToken);

			// Assert
			_repositoryMock.Verify(
				x => x.SearchSignificantChangeProjects(
					It.Is<int>(page => page == 3),
					It.Is<int>(count => count == 25),
					It.Is<string>(keyword => keyword == "school"),
					It.Is<List<string>>(status => status.SequenceEqual(new List<string> { "InProgress" })),
					It.Is<List<string>>(assignee => assignee.SequenceEqual(new List<string> { "Ste" })),
					It.Is<List<byte>>(tier => tier.SequenceEqual(new List<byte> { 1 })),
					It.Is<List<string>>(route => route.SequenceEqual(new List<string> { "a change" })),
					It.Is<CancellationToken>(token => token == cancellationToken)),
				Times.Once);
		}

		private static SignificantChangeProject CreateProject(int id, int urn, byte tier, string trustName, string trustUkprn, string route, string schoolName)
		{
			return new SignificantChangeProject(SignificantChangeStatus.PreDecision, urn, tier, trustName, trustUkprn, route, schoolName)
			{
				Id = id,
				CreatedOn = DateTime.UtcNow
			};
		}
	}
}
