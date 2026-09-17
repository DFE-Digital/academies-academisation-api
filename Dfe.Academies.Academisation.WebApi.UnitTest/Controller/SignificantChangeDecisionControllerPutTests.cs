using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class SignificantChangeDecisionControllerPutTests
{
	private class UnhandledUpdateResult : CommandResult { }

	private readonly Fixture _fixture = new();
	private readonly Mock<IMediator> _mockMediator = new();

	[Fact]
	public async Task CommandReturnsCommandSuccessResult_ReturnsOkResult()
	{
		var request = _fixture.Create<SignificantChangeUpdateDecisionCommand>();
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandSuccessResult());
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeStatusCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandSuccessResult());

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Put(request, It.IsAny<CancellationToken>());

		result.Should().BeOfType<OkResult>();
		_mockMediator.Verify(c => c.Send(
			It.Is<SignificantChangeStatusCommand>(command =>
				command.Id == request.SignificantChangeProjectId && command.Status == request.Decision),
			It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task CommandReturnsNotFoundCommandResult_ReturnsNotFoundResult()
	{
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new NotFoundCommandResult());

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Put(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>());

		result.Should().BeOfType<NotFoundResult>();
	}

	[Fact]
	public async Task CommandReturnsBadRequestCommandResult_ReturnsBadRequestResult()
	{
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new BadRequestCommandResult());

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Put(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>());

		result.Should().BeOfType<BadRequestResult>();
	}

	[Fact]
	public async Task CommandReturnsCommandValidationErrorResult_ReturnsBadRequestResult()
	{
		var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();

		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandValidationErrorResult(expectedValidationErrors));

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Put(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>());

		var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
		var validationErrors = badRequestResult.Value.Should().BeAssignableTo<IEnumerable<ValidationError>>().Subject;
		validationErrors.Should().Equal(expectedValidationErrors);
	}

	[Fact]
	public async Task CommandReturnsUnhandledUpdateResult_ThrowsException()
	{
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new UnhandledUpdateResult());

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		Func<Task> act = () => subject.Put(It.IsAny<SignificantChangeUpdateDecisionCommand>(), It.IsAny<CancellationToken>());

		await act.Should().ThrowAsync<NotImplementedException>();
	}


	[Fact]
	public async Task NullSignificantChangeProjectId_ReturnsBadRequestAndDoesNotSendCommand()
	{
		var request = new SignificantChangeUpdateDecisionCommand { SignificantChangeProjectId = null };
		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Put(request, default);

		result.Should().BeOfType<BadRequestObjectResult>();
		_mockMediator.Verify(c => c.Send(
			It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()), Times.Never);
	}
}
