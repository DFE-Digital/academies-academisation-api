using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class AdvisoryBoardDecisionUpdateCommandExecuteTests
{
	private class UnhandledCommandResult : CommandResult { }

	private readonly Mock<IAdvisoryBoardDecisionRepository> _mockRepo = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();
	private readonly Mock<IConversionProjectRepository> _mockConversionProjectRepository = new();
	private readonly Mock<ITransferProjectRepository> _mockTransferProjectRepository = new();
	private readonly Fixture _fixture = new();
	public AdvisoryBoardDecisionUpdateCommandExecuteTests()
	{
		var mockContext = new Mock<IUnitOfWork>();
		_mockRepo.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
	}

	[Fact]
	public async Task AdvisoryBoardDecisionIdIsDefault___ReturnsBadResult()
	{
		//Arrange
		var target = new AdvisoryBoardDecisionUpdateCommandHandler(_mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object);

		//Act
		var result = await target.Handle(new(), default);

		//Assert
		Assert.IsType<BadRequestCommandResult>(result);
	}

	[Fact]
	public async Task DataQueryReturnsNull__ReturnsCommandNotFoundResult()
	{
		var target = new AdvisoryBoardDecisionUpdateCommandHandler(_mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object);

		//Act
		var result = await target.Handle(new() { AdvisoryBoardDecisionId = 1 }, default);

		//Assert
		Assert.IsType<NotFoundCommandResult>(result);
	}

	[Fact]
	public async Task DecisionUpdateReturnsUnhandledCommandResult__ThrowsException()
	{
		//Arrange
		_mockDecision
			.Setup(d => d.Update(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new UnhandledCommandResult());

		_mockRepo
			.Setup(d => d.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = new AdvisoryBoardDecisionUpdateCommandHandler(_mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object);

		//Act & Assert
		await Assert.ThrowsAsync<NotImplementedException>(() => target.Handle(new() { AdvisoryBoardDecisionId = 1 }, default));
	}

	[Fact]
	public async Task DomainReturnsValidatorError_DoesNotCallExecuteOnDataCommand()
	{
		_mockDecision
			.Setup(c => c.Update(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new CommandValidationErrorResult(new List<ValidationError>()));

		_mockRepo
			.Setup(q => q.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = new AdvisoryBoardDecisionUpdateCommandHandler(_mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object);

		//Act
		_ = await target.Handle(new() { AdvisoryBoardDecisionId = 1 }, default);

		//Assert
		_mockRepo.Verify(c => c.Update(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Never);
	}

	[Fact]
	public async Task DomainReturnsSuccess___CallsExecuteOnDataCommand()
	{
		_mockDecision
			.Setup(c => c.Update(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new CommandSuccessResult());

		_mockDecision.Setup(d => d.AdvisoryBoardDecisionDetails).Returns(_fixture.Build<AdvisoryBoardDecisionDetails>().With(x => x.TransferProjectId, 1).Create());

		_mockRepo
			.Setup(q => q.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = new AdvisoryBoardDecisionUpdateCommandHandler(_mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object);

		//Act
		_ = await target.Handle(new() { AdvisoryBoardDecisionId = 1}, default);

		//Assert
		_mockRepo.Verify(c => c.Update(It.IsAny<ConversionAdvisoryBoardDecision>()), Times.Once);
	}

	[Fact]
	public async Task DomainReturnsSuccess___ReturnsCommandSuccessResult()
	{
		//Arrange
		_mockDecision
			.Setup(d => d.Update(It.IsAny<AdvisoryBoardDecisionDetails>(), It.IsAny<List<AdvisoryBoardDeferredReasonDetails>>(), It.IsAny<List<AdvisoryBoardDeclinedReasonDetails>>(), It.IsAny<List<AdvisoryBoardWithdrawnReasonDetails>>(), It.IsAny<List<AdvisoryBoardDAORevokedReasonDetails>>()))
			.Returns(new CommandSuccessResult());
		_mockDecision.Setup(d => d.AdvisoryBoardDecisionDetails).Returns(_fixture.Build<AdvisoryBoardDecisionDetails>().With(x => x.TransferProjectId, 1).Create());
		_mockRepo
			.Setup(q => q.GetAdvisoryBoardDecisionById(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = new AdvisoryBoardDecisionUpdateCommandHandler(_mockRepo.Object, _mockConversionProjectRepository.Object, _mockTransferProjectRepository.Object);

		//Act
		var result = await target.Handle(new() { AdvisoryBoardDecisionId = 1 }, default);

		//Assert
		Assert.IsType<CommandSuccessResult>(result);
	}
}
