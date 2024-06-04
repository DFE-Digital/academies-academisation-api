using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class AdvisoryBoardDecisionCreateCommandExecuteTests
{
	public AdvisoryBoardDecisionCreateCommandExecuteTests()
	{
		var mockContext = new Mock<IUnitOfWork>();
		_mockRepo.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
	}
	private class UnhandledCreateResult : CreateResult
	{
		public UnhandledCreateResult() : base(default) { }
	}

	private readonly Fixture _fixture = new();

	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockRepo = new();
	private readonly Mock<IConversionAdvisoryBoardDecisionFactory> _mockDecisionFactory = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();


	[Fact]
	public async Task RequestModelIsValid___CallsExecuteOnDataCommand()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
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

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object);

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
			AdvisoryBoardDecisionDate = details.AdvisoryBoardDecisionDate,
			AcademyOrderDate = details.AcademyOrderDate,
			DecisionMadeBy = details.DecisionMadeBy,
			DecisionMakerName = details.DecisionMakerName
		};

		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
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

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object);

		//Act
		var result = (CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel>)await target.Handle(new(), default);

		//Assert
		Assert.Equivalent(expected, result.Payload);
	}

	[Fact]
	public async Task RequestModelIsInvalid_DoesNotCallExecuteOnDataCommand()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new CreateValidationErrorResult(Enumerable.Empty<ValidationError>()));

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object);

		//Act
		_ = await target.Handle(new(), default);

		//Assert
		_mockRepo.Verify(c => c.Insert(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Never);
	}

	[Fact]
	public async Task FactoryReturnsUnhandledCreateResult___ThrowsException()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new UnhandledCreateResult());

		var target = new AdvisoryBoardDecisionCreateCommandHandler(_mockDecisionFactory.Object, _mockRepo.Object);

		//Act && Assert
		await Assert.ThrowsAsync<NotImplementedException>(() => target.Handle(new AdvisoryBoardDecisionCreateCommand(), default));
	}
}
