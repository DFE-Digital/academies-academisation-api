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

public class SignificantChangeDecisionCommandHandlerTests
{
	private class UnhandledCreateResult() : CreateResult(default);

	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockRepo = new();
	private readonly Mock<IConversionAdvisoryBoardDecisionFactory> _mockDecisionFactory = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();
	private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository = new();
	private readonly Mock<ITransferProjectRepository> _mockTransferProjectRepository = new();
	private readonly Mock<ISignificantChangeProjectRepository> _mockSignificantChangeProjectRepository = new();
	private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
	private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

	public SignificantChangeDecisionCommandHandlerTests()
	{
		_mockRepo.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
		_mockSignificantChangeProjectRepository.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
	}

	[Theory]
	[InlineData(Decision.Approved, AdvisoryBoardDecision.Approved)]
	[InlineData(Decision.Declined, AdvisoryBoardDecision.Declined)]
	[InlineData(Decision.Deferred, AdvisoryBoardDecision.Deferred)]
	[InlineData(Decision.Withdrawn, AdvisoryBoardDecision.Withdrawn)]
	public async Task Handle_MapsDecisionAndPassesSignificantChangeProjectIdToFactory(
		Decision sourceDecision,
		AdvisoryBoardDecision expectedDecision)
	{
		//Arrange
		var projectId = _fixture.Create<int>();
		var command = CreateCommand(projectId, sourceDecision);

		SetupSuccessfulCreate();

		var target = CreateHandler();

		//Act
		_ = await target.Handle(command, default);

		//Assert
		_mockDecisionFactory.Verify(f => f.Create(
			It.Is<AdvisoryBoardDecisionDetails>(d =>
				d.SignificantChangeProjectId == projectId &&
				d.ConversionProjectId == null &&
				d.TransferProjectId == null &&
				d.Decision == expectedDecision &&
				d.ApprovedConditionsSet == command.ApprovedConditionsSet &&
				d.ApprovedConditionsDetails == command.ApprovedConditionsDetails &&
				d.AdvisoryBoardDecisionDate == command.AdvisoryBoardDecisionDate &&
				d.AcademyOrderDate == null &&
				d.DecisionMadeBy == command.DecisionMadeBy &&
				d.DecisionMakerName == command.DecisionMakerName),
			It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(),
			It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(),
			It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(),
			It.Is<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>(r => !r.Any())), Times.Once);
	}

	[Fact]
	public async Task Handle_WhenCreateSucceeds_InsertsDecision()
	{
		SetupSuccessfulCreate();

		var target = CreateHandler();

		_ = await target.Handle(CreateCommand(_fixture.Create<int>(), Decision.Declined), default);

		_mockRepo.Verify(c => c.Insert(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Once);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
	}

	[Fact]
	public async Task Handle_WhenApproved_SetSignificantChangeProjectReadOnly()
	{
		var projectId = _fixture.Create<int>();
		var now = DateTime.UtcNow;
		var project = SignificantChangeProject.Create(
			_fixture.Create<int>(),
			_fixture.Create<byte>(),
			_fixture.Create<string>(),
			_fixture.Create<string>(),
			_fixture.Create<string>(),
			_fixture.Create<string>(),
			now);

		_mockDateTimeProvider.Setup(d => d.Now).Returns(now);
		_mockSignificantChangeProjectRepository
			.Setup(r => r.GetById(projectId))
			.ReturnsAsync(project);

		SetupSuccessfulCreate(new AdvisoryBoardDecisionDetails(
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

		var target = CreateHandler();

		_ = await target.Handle(CreateCommand(projectId, Decision.Approved), default);

		project.ReadOnlyDate.Should().Be(now);

		_mockSignificantChangeProjectRepository.Verify(r => r.Update(project), Times.Once);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
	}

	[Fact]
	public async Task Handle_WhenNotApproved_DoesNotSetSignificantChangeProjectReadOnly()
	{
		var projectId = _fixture.Create<int>();

		SetupSuccessfulCreate(new AdvisoryBoardDecisionDetails(
			null,
			null,
			projectId,
			AdvisoryBoardDecision.Declined,
			null,
			null,
			DateTime.UtcNow.AddDays(-1),
			null,
			DecisionMadeBy.DirectorGeneral,
			_fixture.Create<string>()));

		var target = CreateHandler();

		_ = await target.Handle(CreateCommand(projectId, Decision.Declined), default);

		_mockSignificantChangeProjectRepository.Verify(r => r.GetById(It.IsAny<int>()), Times.Never);
		_mockSignificantChangeProjectRepository.Verify(r => r.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
	}

	[Fact]
	public async Task Handle_WhenValidationError_DoesNotInsert()
	{
		_mockDecisionFactory
			.Setup(f => f.Create(
				It.IsAny<AdvisoryBoardDecisionDetails>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new CreateValidationErrorResult([]));

		var target = CreateHandler();

		var result = await target.Handle(CreateCommand(_fixture.Create<int>(), Decision.Approved), default);

		result.Should().BeOfType<CreateValidationErrorResult>();
		_mockRepo.Verify(c => c.Insert(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Never);
	}

	[Fact]
	public async Task Handle_WhenUnhandledCreateResult_ThrowsException()
	{
		_mockDecisionFactory
			.Setup(f => f.Create(
				It.IsAny<AdvisoryBoardDecisionDetails>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new UnhandledCreateResult());

		var target = CreateHandler();

		Func<Task> act = () =>
			target.Handle(CreateCommand(_fixture.Create<int>(), Decision.Approved), default);
		await act.Should().ThrowAsync<NotImplementedException>();
	}

	[Fact]
	public async Task Handle_WhenDecisionIsInvalid_ThrowsArgumentOutOfRangeException()
	{
		var target = CreateHandler();
		var command = CreateCommand(_fixture.Create<int>(), (Decision)99);

		Func<Task> act = () => target.Handle(command, default);

		await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
	}

	private SignificantChangeDecisionCommand CreateCommand(int projectId, Decision decision) => new()
	{
		SignificantChangeProjectId = projectId,
		Decision = decision,
		ApprovedConditionsSet = decision == Decision.Approved ? true : null,
		ApprovedConditionsDetails = decision == Decision.Approved ? _fixture.Create<string>() : null,
		AdvisoryBoardDecisionDate = DateTime.UtcNow.AddDays(-1),
		DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
		DecisionMakerName = _fixture.Create<string>(),
		DeclinedReasons = [],
		DeferredReasons = [],
		WithdrawnReasons = []
	};

	private void SetupSuccessfulCreate(AdvisoryBoardDecisionDetails? details = null)
	{
		details ??= _fixture.Create<AdvisoryBoardDecisionDetails>();

		_mockDecisionFactory
			.Setup(f => f.Create(
				It.IsAny<AdvisoryBoardDecisionDetails>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new CreateSuccessResult<IConversionAdvisoryBoardDecision>(_mockDecision.Object));

		_mockDecision.SetupGet(d => d.AdvisoryBoardDecisionDetails).Returns(details);
		_mockDecision.SetupGet(d => d.DeferredReasons).Returns(new List<AdvisoryBoardDeferredReasonDetails>());
		_mockDecision.SetupGet(d => d.DeclinedReasons).Returns(new List<AdvisoryBoardDeclinedReasonDetails>());
		_mockDecision.SetupGet(d => d.WithdrawnReasons).Returns(new List<AdvisoryBoardWithdrawnReasonDetails>());
		_mockDecision.SetupGet(d => d.DAORevokedReasons).Returns(new List<AdvisoryBoardDAORevokedReasonDetails>());
	}

	private AdvisoryBoardDecisionCreateCommandHandler CreateHandler() =>
		new(
			_mockDecisionFactory.Object,
			_mockRepo.Object,
			_mockConversionProjectRepository.Object,
			_mockTransferProjectRepository.Object,
			_mockSignificantChangeProjectRepository.Object,
			_mockDateTimeProvider.Object);
}
