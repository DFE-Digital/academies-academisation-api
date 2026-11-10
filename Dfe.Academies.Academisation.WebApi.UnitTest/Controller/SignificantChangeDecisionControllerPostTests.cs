using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public class SignificantChangeDecisionControllerPostTests
{
	private class UnhandledCreateResult() : CreateResult(default);

	private readonly Fixture _fixture = new();
	private readonly Mock<IMediator> _mockMediator = new();

	public SignificantChangeDecisionControllerPostTests()
	{
		_mockMediator
			.Setup(c => c.Send(It.IsAny<GetSignificantChangeProjectByIdQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(SignificantChangeDecisionTestData.CreateProjectWithCompletedTasks());
	}

	[Fact]
	public async Task NullSignificantChangeProjectId_ReturnsBadRequestAndDoesNotSendCommand()
	{
		var request = new SignificantChangeDecisionCommand { SignificantChangeProjectId = null };
		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(request, default);

		result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value
			.Should().Be("SignificantChangeProjectId must not be null");
		_mockMediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task ProjectDoesNotExist_ReturnsNotFoundAndDoesNotSendCommand()
	{
		var request = _fixture.Create<SignificantChangeDecisionCommand>();
		using var cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = cancellationTokenSource.Token;
		_mockMediator
			.Setup(c => c.Send(It.IsAny<GetSignificantChangeProjectByIdQuery>(), cancellationToken))
			.ReturnsAsync((SignificantChangeProjectGetByIdResponse?)null);
		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(request, cancellationToken);

		result.Result.Should().BeOfType<NotFoundResult>();
		_mockMediator.Verify(c => c.Send(
			It.Is<GetSignificantChangeProjectByIdQuery>(query => query.Id == request.SignificantChangeProjectId),
			cancellationToken), Times.Once);
		_mockMediator.VerifyNoOtherCalls();
	}

	[Theory]
	[MemberData(nameof(SignificantChangeDecisionTestData.IncompleteTasks), MemberType = typeof(SignificantChangeDecisionTestData))]
	public async Task ProjectHasIncompleteTask_ReturnsBadRequestAndDoesNotSendCommand(
		Action<SignificantChangeProjectGetByIdResponse, string> setTaskStatus, SignificantChangeTaskStatus status)
	{
		var request = _fixture.Create<SignificantChangeDecisionCommand>();
		var project = SignificantChangeDecisionTestData.CreateProjectWithCompletedTasks();
		setTaskStatus(project, status.ToString());
		project.AllTasksComplete().Should().BeFalse();
		_mockMediator
			.Setup(c => c.Send(It.IsAny<GetSignificantChangeProjectByIdQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(project);
		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(request, default);

		result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value
			.Should().Be("All tasks must be complete before a decision can be made");
		_mockMediator.Verify(c => c.Send(
			It.Is<GetSignificantChangeProjectByIdQuery>(query => query.Id == request.SignificantChangeProjectId),
			It.IsAny<CancellationToken>()), Times.Once);
		_mockMediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task ProjectHasNoAssignedUser_ReturnsBadRequestAndDoesNotSendCommand()
	{
		var request = _fixture.Create<SignificantChangeDecisionCommand>();
		var project = SignificantChangeDecisionTestData.CreateProjectWithCompletedTasks();
		project.AssignedUser = null;
		project.AllTasksComplete().Should().BeTrue();
		using var cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = cancellationTokenSource.Token;
		_mockMediator
			.Setup(c => c.Send(It.IsAny<GetSignificantChangeProjectByIdQuery>(), cancellationToken))
			.ReturnsAsync(project);
		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(request, cancellationToken);

		result.Result.Should().BeOfType<BadRequestObjectResult>().Subject.Value
			.Should().Be("Project must have an assigned user");
		_mockMediator.Verify(c => c.Send(
			It.Is<GetSignificantChangeProjectByIdQuery>(query => query.Id == request.SignificantChangeProjectId),
			cancellationToken), Times.Once);
		_mockMediator.VerifyNoOtherCalls();
	}

	[Fact]
	public async Task CommandReturnsCreateSuccessResult_ReturnsCreatedAtRouteResult()
	{
		var decisionServiceModel = _fixture.Create<SignificantChangeDecisionServiceModel>();
		var request = _fixture.Create<SignificantChangeDecisionCommand>();

		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CreateSuccessResult<SignificantChangeDecisionServiceModel>(decisionServiceModel));
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeStatusCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CommandSuccessResult());

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(request, default);

		result.Result.Should().BeOfType<CreatedAtRouteResult>();
		var createdResult = (CreatedAtRouteResult)result.Result!;

		createdResult.Value.Should().BeOfType<SignificantChangeDecisionServiceModel>();
		createdResult.Value.Should().BeEquivalentTo(decisionServiceModel);
		createdResult.RouteName.Should().Be("GetProject");
		createdResult.RouteValues.Should().ContainKey("projectId");
		_mockMediator.Verify(c => c.Send(
			It.Is<SignificantChangeStatusCommand>(command =>
				command.Id == request.SignificantChangeProjectId && command.Status == request.Decision),
			It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
    public async Task CommandReturnsCreateValidationErrorResult_ReturnsBadRequestResult()
    {
        var request = _fixture.Create<SignificantChangeDecisionCommand>();
        var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();

        _mockMediator
        .Setup(c => c.Send(It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new CreateValidationErrorResult(expectedValidationErrors));

        var subject = new SignificantChangeDecisionController(_mockMediator.Object);

        var result = await subject.Post(request, default);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result.Result!;
        badRequestResult.Value.Should().BeAssignableTo<IEnumerable<ValidationError>>();
        var validationErrors = (IEnumerable<ValidationError>)badRequestResult.Value!;
        validationErrors.Should().Equal(expectedValidationErrors);
        _mockMediator.Verify(c => c.Send(
            It.IsAny<SignificantChangeStatusCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

	[Fact]
    public async Task CommandReturnsUnhandledCreateResult_ThrowsException()
    {
        var request = _fixture.Create<SignificantChangeDecisionCommand>();

        _mockMediator
        .Setup(c => c.Send(It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new UnhandledCreateResult());

        var subject = new SignificantChangeDecisionController(_mockMediator.Object);

        Func<Task> act = async () => await subject.Post(request, default);
        await act.Should().ThrowAsync<NotImplementedException>();
    }
}
