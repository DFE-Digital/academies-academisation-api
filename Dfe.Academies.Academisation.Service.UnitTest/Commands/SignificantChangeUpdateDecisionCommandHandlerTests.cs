using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class SignificantChangeUpdateDecisionCommandHandlerTests
{
	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockRepo = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();
	private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository = new();
	private readonly Mock<ITransferProjectRepository> _mockTransferProjectRepository = new();
	private readonly Mock<ISignificantChangeProjectRepository> _mockSignificantChangeProjectRepository = new();
	private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
	private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

	public SignificantChangeUpdateDecisionCommandHandlerTests()
	{
		_mockRepo.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
		_mockSignificantChangeProjectRepository.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
	}

	[Theory]
	[InlineData(Decision.Approved, AdvisoryBoardDecision.Approved)]
	[InlineData(Decision.Declined, AdvisoryBoardDecision.Declined)]
	[InlineData(Decision.Deferred, AdvisoryBoardDecision.Deferred)]
	[InlineData(Decision.Withdrawn, AdvisoryBoardDecision.Withdrawn)]
	public async Task Handle_MapsDecisionAndPassesSignificantChangeProjectIdToDomain(
		Decision sourceDecision,
		AdvisoryBoardDecision expectedDecision)
	{
		var command = CreateCommand(_fixture.Create<int>(), sourceDecision);

		_mockDecision
			.Setup(d => d.Update(
				It.IsAny<AdvisoryBoardDecisionDetails>(),
				It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new CommandSuccessResult());

		_mockDecision.SetupGet(d => d.AdvisoryBoardDecisionDetails).Returns(_fixture.Build<AdvisoryBoardDecisionDetails>()
			.With(x => x.SignificantChangeProjectId, command.SignificantChangeProjectId)
			.With(x => x.ConversionProjectId, (int?)null)
			.With(x => x.TransferProjectId, (int?)null)
			.With(x => x.Decision, expectedDecision)
			.Create());

		_mockRepo
			.Setup(r => r.GetAdvisoryBoardDecisionById(command.AdvisoryBoardDecisionId))
			.ReturnsAsync(_mockDecision.Object);

		var target = CreateHandler();

		_ = await target.Handle(command, default);

		_mockDecision.Verify(d => d.Update(
			It.Is<AdvisoryBoardDecisionDetails>(details =>
				details.SignificantChangeProjectId == command.SignificantChangeProjectId &&
				details.ConversionProjectId == null &&
				details.TransferProjectId == null &&
				details.Decision == expectedDecision &&
				details.ApprovedConditionsSet == command.ApprovedConditionsSet &&
				details.ApprovedConditionsDetails == command.ApprovedConditionsDetails &&
				details.AdvisoryBoardDecisionDate == command.DecisionDate &&
				details.AcademyOrderDate == null &&
				details.DecisionMadeBy == command.DecisionMadeBy &&
				details.DecisionMakerName == command.DecisionMakerName),
			It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(),
			It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(),
			It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(),
			It.Is<List<AdvisoryBoardDAORevokedReasonDetails>>(r => r.Count == 0),
			It.Is<List<AdvisoryBoardDAONotIssuedReasonDetails>>(r=> r.Count == 0)), Times.Once);
	}

	[Fact]
	public async Task Handle_WhenApproved_SetSignificantChangeProjectReadOnly()
	{
		var projectId = _fixture.Create<int>();
		var now = DateTime.UtcNow;
		var project = SignificantChangeProject.Create(
			new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			),
			now);

		_mockDateTimeProvider.Setup(d => d.Now).Returns(now);
		_mockSignificantChangeProjectRepository
			.Setup(r => r.GetById(projectId))
			.ReturnsAsync(project);

		_mockDecision
			.Setup(d => d.Update(
				It.IsAny<AdvisoryBoardDecisionDetails>(),
				It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>(),
				It.IsAny<List<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new CommandSuccessResult());

		_mockDecision.SetupGet(d => d.AdvisoryBoardDecisionDetails).Returns(new AdvisoryBoardDecisionDetails(
			null,
			null,
			projectId,
			AdvisoryBoardDecision.Approved,
			true,
			_fixture.Create<string>(),
			now.AddDays(-1),
			null,
			DecisionMadeBy.DirectorGeneral,
			_fixture.Create<string>()));

		_mockRepo
			.Setup(r => r.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = CreateHandler();

		_ = await target.Handle(CreateCommand(projectId, Decision.Approved), default);

		project.ReadOnlyDate.Should().Be(now);
		_mockSignificantChangeProjectRepository.Verify(r => r.Update(project), Times.Once);
	}

	[Fact]
	public async Task Handle_WhenDecisionIsInvalid_ThrowsArgumentOutOfRangeException()
	{
		_mockRepo
			.Setup(r => r.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = CreateHandler();
		var command = CreateCommand(_fixture.Create<int>(), (Decision)99);

		Func<Task> act = () => target.Handle(command, default);

		await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
	}

	private SignificantChangeUpdateDecisionCommand CreateCommand(int projectId, Decision decision) => new()
	{
		AdvisoryBoardDecisionId = _fixture.Create<int>(),
		SignificantChangeProjectId = projectId,
		Decision = decision,
		ApprovedConditionsSet = decision == Decision.Approved ? true : null,
		ApprovedConditionsDetails = decision == Decision.Approved ? _fixture.Create<string>() : null,
		DecisionDate = DateTime.UtcNow.AddDays(-1),
		DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
		DecisionMakerName = _fixture.Create<string>(),
		DeclinedReasons = [],
		DeferredReasons = [],
		WithdrawnReasons = []
	};

	private AdvisoryBoardDecisionUpdateCommandHandler CreateHandler() =>
		new(
			_mockRepo.Object,
			_mockConversionProjectRepository.Object,
			_mockTransferProjectRepository.Object,
			_mockSignificantChangeProjectRepository.Object,
			_mockDateTimeProvider.Object);
}
