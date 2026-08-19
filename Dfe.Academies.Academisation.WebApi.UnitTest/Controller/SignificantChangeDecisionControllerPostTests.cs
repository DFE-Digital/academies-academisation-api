using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChangeDecision;
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

	[Fact]
	public async Task CommandReturnsCreateSuccessResult_ReturnsCreatedAtRouteResult()
	{
		var decisionServiceModel = _fixture.Create<SignificantChangeDecisionServiceModel>();

		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CreateSuccessResult<SignificantChangeDecisionServiceModel>(decisionServiceModel));

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(It.IsAny<SignificantChangeDecisionCommand>(), default);

		result.Result.Should().BeOfType<CreatedAtRouteResult>();
		var createdResult = (CreatedAtRouteResult)result.Result!;

		createdResult.Value.Should().BeOfType<SignificantChangeDecisionServiceModel>();
		createdResult.Value.Should().BeEquivalentTo(decisionServiceModel);
		createdResult.RouteName.Should().Be("GetProject");
		createdResult.RouteValues.Should().ContainKey("projectId");
	}

	[Fact]
	public async Task CommandReturnsCreateValidationErrorResult_ReturnsBadRequestResult()
	{
		var expectedValidationErrors = _fixture.CreateMany<ValidationError>().ToList();

		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new CreateValidationErrorResult(expectedValidationErrors));

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		var result = await subject.Post(It.IsAny<SignificantChangeDecisionCommand>(), default);

		result.Result.Should().BeOfType<BadRequestObjectResult>();
		var badRequestResult = (BadRequestObjectResult)result.Result!;
		badRequestResult.Value.Should().BeAssignableTo<IEnumerable<ValidationError>>();
		var validationErrors = (IEnumerable<ValidationError>)badRequestResult.Value!;
		validationErrors.Should().Equal(expectedValidationErrors);
	}

	[Fact]
	public async Task CommandReturnsUnhandledCreateResult_ThrowsException()
	{
		_mockMediator
			.Setup(c => c.Send(It.IsAny<SignificantChangeDecisionCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new UnhandledCreateResult());

		var subject = new SignificantChangeDecisionController(_mockMediator.Object);

		Func<Task> act = async () => await subject.Post(It.IsAny<SignificantChangeDecisionCommand>(), default);
		await act.Should().ThrowAsync<NotImplementedException>();
	}
}
