using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.Service.Commands.AdvisoryBoardDecision;
using Dfe.Academies.Academisation.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class ConversionAdvisoryBoardDecisionControllerPutTests
{
	private class UnhandledUpdateResult : CommandResult { }

	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionQueryService> _mockGetQuery = new();
	private readonly Mock<IMediator> _mockMediator = new();


	[Fact]
	public async Task CommandReturnsCommandSuccessResult___ReturnsOkResult()
	{
		//Arrange
		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandSuccessResult());

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.Put(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>());

		//Assert
		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async Task CommandReturnsBadRequestCommandResult___ReturnsBadRequestResult()
	{
		//Arrange
		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new BadRequestCommandResult());

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.Put(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>());

		//Assert
		Assert.IsType<BadRequestResult>(result);
	}

	[Fact]
	public async Task CommandReturnsCommandValidationErrorResult___ReturnsBadRequestResult()
	{
		var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();

		//Arrange
		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandValidationErrorResult(expectedValidationErrors));

		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act
		var result = await subject.Put(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>());

		//Assert
		var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
		var validationErrors = Assert.IsAssignableFrom<IEnumerable<ValidationError>>(badRequestResult.Value);
		Assert.Equal(expectedValidationErrors, validationErrors);
	}

	[Fact]
	public async Task CommandReturnsCreateUnhandledUpdateResult___ThrowsException()
	{
		_mockMediator
			.Setup(c => c.Send(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new UnhandledUpdateResult());

		//Arrange
		var subject = new AdvisoryBoardDecisionController(
			_mockMediator.Object,
			_mockGetQuery.Object);

		//Act && Assert
		await Assert.ThrowsAsync<NotImplementedException>(
			() => subject.Put(It.IsAny<AdvisoryBoardDecisionUpdateCommand>(), It.IsAny<CancellationToken>()));
	}
}
