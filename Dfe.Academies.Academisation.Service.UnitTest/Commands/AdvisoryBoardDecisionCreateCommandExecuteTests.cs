using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class AdvisoryBoardDecisionCreateCommandExecuteTests
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

	public AdvisoryBoardDecisionCreateCommandExecuteTests()
	{
		_mockRepo.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
		_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
		_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(_mockUnitOfWork.Object);
	}
	
	[Fact]
	public async Task RequestModelIsValid___CallsExecuteOnDataCommand()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new CreateSuccessResult<IConversionAdvisoryBoardDecision>(_mockDecision.Object));

		_mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(_fixture.Create<AdvisoryBoardDecisionDetails>());
		_mockDecision
			.SetupGet(d => d.DeferredReasons)
			.Returns(new List<AdvisoryBoardDeferredReasonDetails>());
		_mockDecision
			.SetupGet(d => d.DeclinedReasons)
			.Returns(new List<AdvisoryBoardDeclinedReasonDetails>());
		_mockDecision
			.SetupGet(d => d.WithdrawnReasons)
			.Returns(new List<AdvisoryBoardWithdrawnReasonDetails>());
		_mockDecision
			.SetupGet(d => d.DAORevokedReasons)
			.Returns(new List<AdvisoryBoardDAORevokedReasonDetails>());
		_mockDecision
			.SetupGet(d => d.DAONotIssuedReasons)
			.Returns(new List<AdvisoryBoardDAONotIssuedReasonDetails>());

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object, _mockSignificantChangeProjectRepository.Object, _mockDateTimeProvider.Object);

		//Act
		_ = await target.Handle(new AdvisoryBoardDecisionCreateCommand(), default);

		//Assert
		_mockRepo.Verify(c => c.Insert(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Once);
	}

	[Fact]
	public async Task RequestModelIsValid___ReturnsExpectedConversionAdvisoryBoardDecisionServiceModel()
	{
		//Arrange
		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();
		var deferred = _fixture.CreateMany<AdvisoryBoardDeferredReasonDetails>();
		var declined = _fixture.CreateMany<AdvisoryBoardDeclinedReasonDetails>();
		var withdrawn = _fixture.CreateMany<AdvisoryBoardWithdrawnReasonDetails>();
		var daoRevoked = _fixture.CreateMany<AdvisoryBoardDAORevokedReasonDetails>();
		var daoNotIssued = _fixture.CreateMany<AdvisoryBoardDAONotIssuedReasonDetails>();

		var expected = new ConversionAdvisoryBoardDecisionServiceModel
		{
			ConversionProjectId = details.ConversionProjectId,
			Decision = details.Decision,
			ApprovedConditionsSet = details.ApprovedConditionsSet,
			ApprovedConditionsDetails = details.ApprovedConditionsDetails,
			DeclinedReasons = declined.ToList(),
			DeferredReasons = deferred.ToList(),
			WithdrawnReasons = withdrawn.ToList(),
			DAORevokedReasons = daoRevoked.ToList(),
			DAONotIssuedReasons = daoNotIssued.ToList(),
			AdvisoryBoardDecisionDate = details.AdvisoryBoardDecisionDate,
			AcademyOrderDate = details.AcademyOrderDate,
			DecisionMadeBy = details.DecisionMadeBy,
			DecisionMakerName = details.DecisionMakerName
		};

		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new CreateSuccessResult<IConversionAdvisoryBoardDecision>(_mockDecision.Object));

		_mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(details);
		_mockDecision
			.SetupGet(d => d.DeferredReasons)
			.Returns(deferred.ToList().AsReadOnly());
		_mockDecision
			.SetupGet(d => d.DeclinedReasons)
			.Returns(declined.ToList().AsReadOnly());
		_mockDecision
			.SetupGet(d => d.WithdrawnReasons)
			.Returns(withdrawn.ToList().AsReadOnly());
		_mockDecision
			.SetupGet(d => d.DAORevokedReasons)
			.Returns(daoRevoked.ToList().AsReadOnly());
		_mockDecision
			.SetupGet(d => d.DAONotIssuedReasons)
			.Returns(daoNotIssued.ToList().AsReadOnly());

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object, _mockSignificantChangeProjectRepository.Object, _mockDateTimeProvider.Object);

		//Act
		var result = (CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel>)await target.Handle(new AdvisoryBoardDecisionCreateCommand(), default);

		//Assert
		Assert.Equivalent(expected, result.Payload);
	}

	
	[Fact]
	public async Task RequestModelIsInvalid_DoesNotCallExecuteOnDataCommand()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new CreateValidationErrorResult(Enumerable.Empty<ValidationError>()));

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object, _mockSignificantChangeProjectRepository.Object, _mockDateTimeProvider.Object);

		//Act
		_ = await target.Handle(new AdvisoryBoardDecisionCreateCommand(), default);

		//Assert
		_mockRepo.Verify(c => c.Insert(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Never);
	}

	[Fact]
	public async Task FactoryReturnsUnhandledCreateResult___ThrowsException()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>(), It.IsAny<IEnumerable<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new UnhandledCreateResult());

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object , _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object, _mockSignificantChangeProjectRepository.Object, _mockDateTimeProvider.Object);

		//Act && Assert
		await Assert.ThrowsAsync<NotImplementedException>(() => target.Handle(new AdvisoryBoardDecisionCreateCommand(), default));
	}

	[Fact]
	public async Task Handle_ConversionApprovedWhenApproved_SetConversionProjectToReadOnly()
	{
		var projectId = _fixture.Create<int>();
		var now = DateTime.UtcNow;

		var project = _fixture.Create<Project>();

		_mockDateTimeProvider.Setup(d => d.Now).Returns(now);
		_mockConversionProjectRepository
			.Setup(r => r.GetById(projectId))
			.ReturnsAsync(project);

		SetupSuccessfulCreate(new AdvisoryBoardDecisionDetails(
			projectId,
			null,
			null,
			AdvisoryBoardDecision.Approved,
			true,
			_fixture.Create<string>(),
			now.AddDays(-1),
			null,
			DecisionMadeBy.DirectorGeneral,
			_fixture.Create<string>()));

		var target = CreateHandler();

		_ = await target.Handle(CreateCommand(AdvisoryBoardDecision.Approved, projectId), default);

		project.ReadOnlyDate.Should().Be(now);

		_mockConversionProjectRepository.Verify(r => r.Update(project), Times.Once);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
	}

	[Fact]
	public async Task Handle_TransferApprovedWhenApproved_SetTransferProjectToReadOnly()
	{
		var projectId = _fixture.Create<int>();
		var now = DateTime.UtcNow;

		var project = _fixture.Create<Domain.TransferProjectAggregate.TransferProject>();

		_mockDateTimeProvider.Setup(d => d.Now).Returns(now);
		_mockTransferProjectRepository
			.Setup(r => r.GetById(projectId))
			.ReturnsAsync(project);

		SetupSuccessfulCreate(new AdvisoryBoardDecisionDetails(
			null,
			projectId,
			null,
			AdvisoryBoardDecision.Approved,
			true,
			_fixture.Create<string>(),
			now.AddDays(-1),
			null,
			DecisionMadeBy.DirectorGeneral,
			_fixture.Create<string>()));

		var target = CreateHandler();

		_ = await target.Handle(CreateCommand(AdvisoryBoardDecision.Approved, projectId), default);

		project.ReadOnlyDate.Should().Be(now);

		_mockTransferProjectRepository.Verify(r => r.Update(project), Times.Once);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
	}

	private void SetupSuccessfulCreate(AdvisoryBoardDecisionDetails? details = null)
	{
		details ??= _fixture.Create<AdvisoryBoardDecisionDetails>();

		_mockDecisionFactory
			.Setup(f => f.Create(
				It.IsAny<AdvisoryBoardDecisionDetails>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeferredReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDeclinedReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardWithdrawnReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDAORevokedReasonDetails>>(),
				It.IsAny<IEnumerable<AdvisoryBoardDAONotIssuedReasonDetails>>()))
			.Returns(new CreateSuccessResult<IConversionAdvisoryBoardDecision>(_mockDecision.Object));

		_mockDecision.SetupGet(d => d.AdvisoryBoardDecisionDetails).Returns(details);
		_mockDecision.SetupGet(d => d.DeferredReasons).Returns(new List<AdvisoryBoardDeferredReasonDetails>());
		_mockDecision.SetupGet(d => d.DeclinedReasons).Returns(new List<AdvisoryBoardDeclinedReasonDetails>());
		_mockDecision.SetupGet(d => d.WithdrawnReasons).Returns(new List<AdvisoryBoardWithdrawnReasonDetails>());
		_mockDecision.SetupGet(d => d.DAORevokedReasons).Returns(new List<AdvisoryBoardDAORevokedReasonDetails>());
		_mockDecision.SetupGet(d => d.DAONotIssuedReasons).Returns(new List<AdvisoryBoardDAONotIssuedReasonDetails>());
	}

	private AdvisoryBoardDecisionCreateCommand CreateCommand(AdvisoryBoardDecision decision, int? conversionProjectId = null, int? transferProjectId = null) => new()
	{
		ConversionProjectId = conversionProjectId,
		TransferProjectId = transferProjectId,
		Decision = decision,
		AdvisoryBoardDecisionDate = DateTime.UtcNow.AddDays(-1),
		DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
		DecisionMakerName = _fixture.Create<string>(),
		DeclinedReasons = [],
		DeferredReasons = [],
		WithdrawnReasons = [],
		DAORevokedReasons = []
	};

	private AdvisoryBoardDecisionCreateCommandHandler CreateHandler() =>
		new(
			_mockDecisionFactory.Object,
			_mockRepo.Object,
			_mockConversionProjectRepository.Object,
			_mockTransferProjectRepository.Object,
			_mockSignificantChangeProjectRepository.Object,
			_mockDateTimeProvider.Object);
}
