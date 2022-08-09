using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Service.Commands;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class AdvisoryBoardDecisionUpdateCommandExecuteTests
{
	private class UnhandledCommandResult : CommandResult { }

	private readonly Mock<IAdvisoryBoardDecisionGetDataByDecisionIdQuery> _mockDataQuery = new();
	private readonly Mock<IAdvisoryBoardDecisionUpdateDataCommand> _mockDataCommand = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();

	[Fact]
	public async Task AdvisoryBoardDecisionIdIsDefault___ReturnsBadResult()
	{
		//Arrange
		var target = new AdvisoryBoardDecisionUpdateCommand(_mockDataCommand.Object, _mockDataQuery.Object);

		//Act
		var result = await target.Execute(new());
			
		//Assert
		Assert.IsType<BadRequestCommandResult>(result);
	}
	
	[Fact]
	public async Task DataQueryReturnsNull__ReturnsCommandNotFoundResult()
	{
		var target = new AdvisoryBoardDecisionUpdateCommand(_mockDataCommand.Object, _mockDataQuery.Object);
			
		//Act
		var result = await target.Execute(new() { AdvisoryBoardDecisionId = 1});
	
		//Assert
		Assert.IsType<NotFoundCommandResult>(result);
	}

	[Fact]
	public async Task DecisionUpdateReturnsUnhandledCommandResult__ThrowsException()
	{
		//Arrange
		_mockDecision
			.Setup(d => d.Update(It.IsAny<AdvisoryBoardDecisionDetails>()))
			.Returns(new UnhandledCommandResult());
			
		_mockDataQuery
			.Setup(d => d.Execute(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);
			
		var target = new AdvisoryBoardDecisionUpdateCommand(_mockDataCommand.Object, _mockDataQuery.Object);
			
		//Act & Assert
		await Assert.ThrowsAsync<NotImplementedException>(() => target.Execute(new() { AdvisoryBoardDecisionId = 1}));
	}
		
	[Fact]
	public async Task DomainReturnsValidatorError_DoesNotCallExecuteOnDataCommand()
	{
		_mockDecision
			.Setup(c => c.Update(It.IsAny<AdvisoryBoardDecisionDetails>()))
			.Returns(new CommandValidationErrorResult(new List<ValidationError>()));

		_mockDataQuery
			.Setup(q => q.Execute(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);
			
		var target = new AdvisoryBoardDecisionUpdateCommand(_mockDataCommand.Object, _mockDataQuery.Object);
			
		//Act
		_ = await target.Execute(new() { AdvisoryBoardDecisionId = 1});
	
		//Assert
		_mockDataCommand.Verify(c => c.Execute(It.IsAny<IConversionAdvisoryBoardDecision>()), Times.Never);
	}
		
	[Fact]
	public async Task DomainReturnsSuccess___CallsExecuteOnDataCommand()
	{
		_mockDecision
			.Setup(c => c.Update(It.IsAny<AdvisoryBoardDecisionDetails>()))
			.Returns(new CommandSuccessResult());

		_mockDataQuery
			.Setup(q => q.Execute(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);

		var target = new AdvisoryBoardDecisionUpdateCommand(_mockDataCommand.Object, _mockDataQuery.Object);
			
		//Act
		_ = await target.Execute(new() { AdvisoryBoardDecisionId = 1});
	
		//Assert
		_mockDataCommand.Verify(c => c.Execute(It.IsAny<IConversionAdvisoryBoardDecision>()), Times.Once);
	}

	[Fact]
	public async Task DomainReturnsSuccess___ReturnsCommandSuccessResult()
	{
		//Arrange
		_mockDecision
			.Setup(d => d.Update(It.IsAny<AdvisoryBoardDecisionDetails>()))
			.Returns(new CommandSuccessResult());

		_mockDataQuery
			.Setup(q => q.Execute(It.IsAny<int>()))
			.ReturnsAsync(_mockDecision.Object);
			
		var target = new AdvisoryBoardDecisionUpdateCommand(_mockDataCommand.Object, _mockDataQuery.Object);

		//Act
		var result = await target.Execute(new() { AdvisoryBoardDecisionId = 1});
	
		//Assert
		Assert.IsType<CommandSuccessResult>(result);
	}
}
