using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.IService.ServiceModels.ConversionAdvisoryBoardDecision;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class AdvisoryBoardDecisionCreateCommandExecuteTests
{
	private class UnhandledCreateResult : CreateResult
	{
		public UnhandledCreateResult() : base(default) { }
	}

	private readonly Fixture _fixture = new();

	private readonly Mock<IAdvisoryBoardDecisionCreateDataCommand> _mockDataCommand = new();
	private readonly Mock<IConversionAdvisoryBoardDecisionFactory> _mockDecisionFactory = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();

	[Fact]
	public async Task RequestModelIsValid___CallsExecuteOnDataCommand()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>()))
			.Returns(new CreateSuccessResult<IConversionAdvisoryBoardDecision>(_mockDecision.Object));

		_mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(_fixture.Create<AdvisoryBoardDecisionDetails>());

		var target = new AdvisoryBoardDecisionCreateCommand(_mockDecisionFactory.Object, _mockDataCommand.Object);

		//Act
		_ = await target.Execute(new AdvisoryBoardDecisionCreateRequestModel());

		//Assert
		_mockDataCommand.Verify(c => c.Execute(It.IsAny<IConversionAdvisoryBoardDecision>()), Times.Once);
	}

	[Fact]
	public async Task RequestModelIsValid___ReturnsExpectedConversionAdvisoryBoardDecisionServiceModel()
	{
		//Arrange
		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();
		var deferred = _fixture.CreateMany<AdvisoryBoardDeferredReasonDetails>();
		var declined = _fixture.CreateMany<AdvisoryBoardDeclinedReasonDetails>();
		var withdrawn = _fixture.CreateMany<AdvisoryBoardWithdrawnReasonDetails>();

		var expected = new ConversionAdvisoryBoardDecisionServiceModel
		{
			ConversionProjectId = details.ConversionProjectId,
			Decision = details.Decision,
			ApprovedConditionsSet = details.ApprovedConditionsSet,
			ApprovedConditionsDetails = details.ApprovedConditionsDetails,
			DeclinedReasons = declined.ToList(),
			DeferredReasons = deferred.ToList(),
			WithdrawnReasons = withdrawn.ToList(),
			AdvisoryBoardDecisionDate = details.AdvisoryBoardDecisionDate,
			AcademyOrderDate = details.AcademyOrderDate,
			DecisionMadeBy = details.DecisionMadeBy
		};

		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>()))
			.Returns(new CreateSuccessResult<IConversionAdvisoryBoardDecision>(_mockDecision.Object));

		_mockDecision
			.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(details);

		var target = new AdvisoryBoardDecisionCreateCommand(_mockDecisionFactory.Object, _mockDataCommand.Object);

		//Act
		var result = (CreateSuccessResult<ConversionAdvisoryBoardDecisionServiceModel>)await target.Execute(new());

		//Assert
		Assert.Equivalent(expected, result.Payload);
	}

	[Fact]
	public async Task RequestModelIsInvalid_DoesNotCallExecuteOnDataCommand()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>()))
			.Returns(new CreateValidationErrorResult(Enumerable.Empty<ValidationError>()));

		var target = new AdvisoryBoardDecisionCreateCommand(_mockDecisionFactory.Object, _mockDataCommand.Object);

		//Act
		_ = await target.Execute(new());

		//Assert
		_mockDataCommand.Verify(c => c.Execute(It.IsAny<IConversionAdvisoryBoardDecision>()), Times.Never);
	}

	[Fact]
	public async Task FactoryReturnsUnhandledCreateResult___ThrowsException()
	{
		//Arrange
		_mockDecisionFactory
			.Setup(f => f.Create(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>()))
			.Returns(new UnhandledCreateResult());

		var target = new AdvisoryBoardDecisionCreateCommand(_mockDecisionFactory.Object, _mockDataCommand.Object);

		//Act && Assert
		await Assert.ThrowsAsync<NotImplementedException>(() => target.Execute(new AdvisoryBoardDecisionCreateRequestModel()));
	}
}
